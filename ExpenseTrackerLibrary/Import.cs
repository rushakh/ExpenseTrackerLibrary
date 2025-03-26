using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ExpenseTrackerLibrary
{
    /// <summary>
    /// A class that contains tools and methods that can be used to import transactions 
    /// from text (*and in the future other formats*) into the database.
    /// The Order of Data should be set beforehand otherwise, by default, it's assumed
    /// that its Amount of money, the title, and then the date.
    /// </summary>
    internal static class Import
    {
        private static string _orderOfData = "A,T,D";
        private static Globals.ImportValues _firstImportValue = Globals.ImportValues.Amount;
        private static Globals.ImportValues _secondImportValue = Globals.ImportValues.Title;
        private static Globals.ImportValues _thirdImportValue = Globals.ImportValues.DateAndTime;
        private static decimal _amount;
        private static string? _title;
        private static DateTime _dateTime;
        internal static Globals.ImportValues ImportValueOrders; // NO

        /// <summary>
        /// Order of the data in the string that is to be imported. Default 
        /// Format is "A,T,D". A is for Amount, T for Title, and D for Date.
        /// Change the order however you want but seperate them by comma without space (case Insensitive).
        /// e.g., "T,A,D" or "D,A,T".
        /// Wrong input will cause all the subsequent actions to fail.
        /// </summary>
        public static string OrderOfData 
        {
            get 
            { 
                return _orderOfData;
            }
            set 
            { 
                _orderOfData = value;
                DetermineTheDataOrder();
            } 
        }

        /// <summary>
        /// Imports a series of transactions using the array provided (every element should be a transaction). The transaction 
        /// type and category should be manually set. The delimiter is the character or string that seperates the amount, the title
        /// and the date from each other. You can also include a string to not be considered (and removed). the importantMarker is the set of
        /// characters that are present in the text to mark it as important (e.g., * or xxx).
        /// </summary>
        /// <param name="textsToImport"></param>
        /// <param name="transactionType"></param>
        /// <param name="category"></param>
        /// <param name="delimiter"></param>
        /// <param name="ignore"></param>
        /// <param name="importantMarker"></param>
        /// <param name="isDDMMYYYYOrReverse"></param>
        /// <returns></returns>
        internal static int ImportTransactions(string[] textsToImport, Globals.TransactionTypes transactionType, Category category, string delimiter, string ignore, string? importantMarker, bool isDDMMYYYYOrReverse = true)
        {
            int successCounter = 0;
            if (textsToImport is null || textsToImport.Length == 0) { throw new ArgumentException(); }
            // *** Can not do them in parallel because they set and use the properties (_amount, _dateTime, and _title)
            // *** ONLY WHEN that is changed, can Parallel be used (though should keep in mind the possible problems with the counter).
            foreach (string importText in textsToImport)
            {
                bool isSuccessful = ImportTransaction(importText, transactionType, category, delimiter, ignore, importantMarker, isDDMMYYYYOrReverse);
                if (isSuccessful) { successCounter++; }
            }
            return successCounter;
        }

        /// <summary>
        /// Imports a transaction using the provided string. The transaction type and category should be manually set.
        /// The delimiter is the character or string that seperates the amount, the title and the date from each other. 
        /// You can also include a string to not be considered (and removed). the importantMarker is the set of
        /// characters that are present in the text to mark it as important (e.g., * or xxx).
        /// </summary>
        /// <param name="textToImport"></param>
        /// <param name="transactionType"></param>
        /// <param name="category"></param>
        /// <param name="delimiter"></param>
        /// <param name="ignore"></param>
        /// <param name="importantMarker"></param>
        /// <param name="isDDMMYYYYOrReverse"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        internal static bool ImportTransaction (string textToImport, Globals.TransactionTypes transactionType , Category category, string delimiter, string ignore, string? importantMarker, bool isDDMMYYYYOrReverse = true)
        {
            if (textToImport == string.Empty) { throw new ArgumentException(); }
            bool isSuccessful = false;
            bool isImportant = false;
            string tempToImportText;
            // removing ignore string
            tempToImportText = RemoveString(textToImport, ignore);
            // checking for important marker
            if (importantMarker is not null && importantMarker != string.Empty && tempToImportText.Contains(importantMarker)) { isImportant = true; }
            // Gettintg the values
            string[] dataToBeImported = tempToImportText.Split(delimiter);           
            if (dataToBeImported.Length == 3)
            {
                try
                {
                    SetData(dataToBeImported[0], _firstImportValue);
                    SetData(dataToBeImported[1], _secondImportValue);
                    SetData(dataToBeImported[2], _thirdImportValue);
                }
                catch (ArgumentException)
                {
                    (_dateTime, _amount, _title) = ExtractTransactionData(tempToImportText, isDDMMYYYYOrReverse);
                }
            }
            else
            {
                (_dateTime, _amount, _title) = ExtractTransactionData(tempToImportText, isDDMMYYYYOrReverse);
            }
            // Importing
            Transaction importTransaction = new Transaction(_dateTime, _amount, transactionType, isImportant, null, category, _title, null, null);
            // Having an Id means that it has been added to the database.
            if (importTransaction.Id != 0) { isSuccessful = true; }
            return isSuccessful;
        }
       
        /// <summary>
        /// Extracts and returns the date time, the amount of money, and the title (practically everything that is not date or amount) 
        /// from the import text.
        /// </summary>
        /// <param name="textToImport"></param>
        /// <param name="isDDMMYYYYOrReverse"></param>
        /// <returns></returns>
        private static (DateTime, decimal, string?) ExtractTransactionData (string textToImport, bool isDDMMYYYYOrReverse = true)
        {
            DateTime dateAndTime;
            string dateAndTimeString, amountString;
            string? title;
            decimal amount;
            // Since amount will parse numbers, it can by mistake parse Date as number as well
            // so we always get Date first thing.
            // DateTime
            (dateAndTime, dateAndTimeString) = GetDateTime(textToImport, isDDMMYYYYOrReverse);
            string amountAndTitleString = RemoveString(textToImport, dateAndTimeString);            
            // Amount
            (amount, amountString) = GetAmount(amountAndTitleString);
            string titleAndMiscString = RemoveString(amountAndTitleString, amountString);            
            // Title
            // Everything else is assumed to be the title as it would be
            // too complicated and impractical to seperate title, note, etc from each other.
            // It can even be empty.
            title = titleAndMiscString;           
            return (dateAndTime, amount, title);
        }

        /// <summary>
        /// Gets the amount of money stated in a string (whole number or decimal) and returns it in 
        /// addition to the specified string that the number was parsed from.
        /// </summary>
        /// <param name="amountText"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        private static (decimal, string) GetAmount (string amountText)
        {
            decimal amount;
            string amountString;
            string regPattern = @"(\d+(?:\.\d{1,2})?)";
            // 123.22 or -2.30
            if (Regex.IsMatch (amountText, regPattern))
            {
                var regxObj = new Regex(regPattern);
                var result = regxObj.Match(amountText);
                amountString = result.Value;
                amount = decimal.Parse (result.Value);
            }
            else if (decimal.TryParse(amountText, out amount))
            {
                amount = decimal.Parse(amountText);
                amountString = amount.ToString();
            }
            else
            {
                throw new ArgumentException();
            }
                return (amount, amountString);
        }

        /// <summary>
        /// Gets and returns the first matching DateTime and the corresponding string from the inputed text. 
        /// By default, It's considered that the date is in dd/MM/yyyy or yyyy/MM/dd format. 
        /// If that is not the case set the bool to false.
        /// </summary>
        /// <param name="dateText"></param>
        /// <param name="isDDMMYYYYOrReverse"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        private static (DateTime, string) GetDateTime (string dateText, bool isDDMMYYYYOrReverse = true)
        {
            DateTime dateAndTime;
            string dateString;
            if (isDDMMYYYYOrReverse)
            {
                // dd/mm/yyyy e.g., 24.03.1756 OR 24/03/1756 OR 24-03-1756
                if (Regex.IsMatch(dateText, @"(\d{1,2}[./-]\d{1,2}[./-]\d{4})"))
                {
                    Regex regxObj = new Regex(@"(\d{1,2}[./-]\d{1,2}[./-]\d{4})");
                    var result = regxObj.Match(dateText);
                    dateString = result.Value;
                    string tempDate = result.Value.Replace('-', '/').Replace('.', '/');
                    dateAndTime = DateTime.ParseExact(tempDate, "dd/MM/yyyy" , new CultureInfo ("en-GB"));
                    //dateAndTime = DateTime.Parse(tempDate);
                }
                // yyyy/mm/dd
                else if (Regex.IsMatch(dateText, @"(\d{4})[./-]\d{1,2}[./-]\d{1,2}"))
                {
                    Regex regxObj = new Regex(@"(\d{4})[./-]\d{1,2}[./-]\d{1,2}");
                    var result = regxObj.Match(dateText);
                    dateString = result.Value;
                    string tempDate = result.Value.Replace('-', '/').Replace('.', '/');
                    dateAndTime = DateTime.ParseExact(tempDate, "yyyy/MM/dd", new CultureInfo("en-GB"));
                    //dateAndTime = DateTime.Parse(tempDate);
                }
                else
                {
                    throw new ArgumentException();
                }
            }
            // MM/dd/yyyy American style e.g., 03/24/1756
            else
            {
                if (Regex.IsMatch(dateText, @"(\d{1,2}[./-]\d{1,2}[./-]\d{4})"))
                {
                    Regex regxObj = new Regex(@"(\d{1,2}[./-]\d{1,2}[./-]\d{4})");
                    var result = regxObj.Match(dateText);
                    dateString = result.Value;
                    string tempDate = result.Value.Replace('-', '/').Replace('.', '/');
                    dateAndTime = DateTime.ParseExact(tempDate, "MM/dd/yyyy", new CultureInfo ("en-US"));
                    //dateAndTime = DateTime.Parse(tempDate);
                }
                else
                {
                    throw new ArgumentException();
                }
            }
                return (dateAndTime,dateString);
        }

        /// <summary>
        /// Sets the Import Data value properties. The datetime, the amount and the title.
        /// </summary>
        /// <param name="dataText"></param>
        /// <param name="numberedImportValue"></param>
        /// <exception cref="ArgumentException"></exception>
        private static void SetData(string dataText, Globals.ImportValues numberedImportValue)
        {
            // determine what it is by getting the enum int and
            // put it through a switch statement to assign a value to
            // the property it should.
            int enumValue = (int)numberedImportValue;
            switch (enumValue)
            {
                case 1:
                    // 1 means it's meant for Amount, therefore decimal
                    string tempString;
                    (_amount, tempString) = GetAmount(dataText);
                    break;
                case 2:
                    // 2 means it's meant for DateAndTime, therefore DateTime
                    string tempText;
                    (_dateTime, tempText) = GetDateTime(dataText);
                    break;
                case 3:
                    // 3 means it's for Title, therefore string, so will just assign it as it is
                    _title = dataText;
                    break;
                default:
                    // if it's none of the above then something is wrong
                    throw new ArgumentException();
                    break;
            }
        }

        /// <summary>
        /// Gets the data string (either A, T, or D) and returns the corresponding enum.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        private static Globals.ImportValues GetDataEnum (string data)
        {
            switch (data)
            {
                case "A":
                    return Globals.ImportValues.Amount;
                    break;
                case "T":
                    return Globals.ImportValues.Title;
                    break;
                case "D":
                    return Globals.ImportValues.DateAndTime;
                    break;
                default:
                    throw new ArgumentException();
                    break;
            }
        }

        /// <summary>
        /// Determines the data order that should be used in reading and importing of 
        /// data based on the OrderOfData string.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        private static void DetermineTheDataOrder()
        {
            string[] dataOrder = OrderOfData.Split(",");
            if (dataOrder.Length == 3)
            {
                int count = 0;
                foreach (string data in dataOrder)
                {
                    switch (count)
                    {
                        case 0:
                            _firstImportValue = GetDataEnum(data);
                            break;
                        case 1:
                            _secondImportValue = GetDataEnum(data);
                            break;
                        case 2:
                            _thirdImportValue = GetDataEnum(data);
                            break;
                    }
                    count++;
                }
            }
            else
            {
                throw new ArgumentException();
            }
        }

        /// <summary>
        /// Simply removes a string from a string (all instances of it). Additionally,
        /// removes all trailing and leading white space characters
        /// </summary>
        /// <param name="completeString"></param>
        /// <param name="toBeRemoved"></param>
        /// <returns></returns>
        private static string RemoveString (string completeString, string toBeRemoved)
        {
            string editedString = completeString.Replace(toBeRemoved, string.Empty);
            editedString.Trim();
            return editedString;
        }
    }
}

using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ExpenseTrackerLibrary.Globals;

namespace ExpenseTrackerLibrary
{
    /// <summary>
    /// A class that contains tools for converting the SQLiteReader of the database to types of objects 
    /// required for this library.
    /// </summary>
    internal static class ConvertSQLiteReader
    {       
        /// <summary>
        /// Constructs a Transaction object using the database element in the SQLite Reader and returns Transaction.
        /// Converts only the first element in the reader if there are more than one.
        /// </summary>
        /// <param name="sqliteDataReader"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        internal static Transaction ToTransaction(SqliteDataReader sqliteDataReader)
        {
            if (sqliteDataReader.HasRows)
            {
                Transaction loadedTransaction;
                sqliteDataReader.Read();
                int id = sqliteDataReader.GetInt32(0);
                DateTime dateTime = DateTime.Parse (sqliteDataReader[1].ToString(), CultureInfo.GetCultureInfo("en-GB"));
                decimal amount = (decimal)sqliteDataReader.GetDouble(2);
                Globals.TransactionTypes transactionType = (TransactionTypes)sqliteDataReader.GetInt32(3);
                // GetBoolean method does not work, had to use a roundabout way
                bool isImportant = false;
                if ((string)sqliteDataReader[4] == bool.TrueString) { isImportant = true; }
                string[] keywords = GetKeywordsForTransaction(sqliteDataReader.GetString(5));
                Category category = Globals.Database.Reader.GetCategory(sqliteDataReader.GetInt32(6));
                string? title = sqliteDataReader.GetString(7);
                string? note = sqliteDataReader.GetString(8);
                string? imagePath = sqliteDataReader.GetString(9);
                loadedTransaction = new Transaction(id, dateTime, amount, transactionType, isImportant, keywords, category, title, note, imagePath);
                return loadedTransaction;
            }
            else
            {
                throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Constructs Transaction objects using the database elements in the SQLite Reader and returns Transaction[].
        /// </summary>
        /// <param name="sqliteDataReader"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        internal static Transaction[] ToTransactions(SqliteDataReader sqliteDataReader)
        {
            if (sqliteDataReader.HasRows)
            {
                List<Transaction> transactions = new List<Transaction>();
                while (sqliteDataReader.Read())
                {
                    int id = sqliteDataReader.GetInt32(0);
                    //DateTime dateTime = sqliteDataReader.GetDateTime(1);
                    DateTime dateTime = DateTime.Parse(sqliteDataReader[1].ToString(), CultureInfo.GetCultureInfo("en-GB"));
                    decimal amount = (decimal)sqliteDataReader.GetDouble(2);
                    Globals.TransactionTypes transactionType = (TransactionTypes)sqliteDataReader.GetInt32(3);
                    // GetBoolean method does not work, had to use a roundabout way
                    bool isImportant = false;
                    if ((string)sqliteDataReader[4] == bool.TrueString) { isImportant = true; }
                    string[] keywords = GetKeywordsForTransaction(sqliteDataReader.GetString(5));
                    Category category = Globals.Database.Reader.GetCategory(sqliteDataReader.GetInt32(6));
                    string? title = sqliteDataReader.GetString(7);
                    string? note = sqliteDataReader.GetString(8);
                    string? imagePath = sqliteDataReader.GetString(9);
                    Transaction loadedTransaction = new Transaction(id, dateTime, amount, transactionType, isImportant, keywords, category, title, note, imagePath);
                    transactions.Add(loadedTransaction);
                }
                return transactions.ToArray();
            }
            else
            {
                throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Returns a string[] populated by seperating the aggregated keywords in the given string.
        /// </summary>
        /// <param name="keywords"></param>
        /// <returns></returns>
        private static string[] GetKeywordsForTransaction (string keywords)
        {
            string[] tempKeywords = keywords.Split(", ", StringSplitOptions.TrimEntries);
            return tempKeywords;
        }

        /// <summary>
        /// Constructs a Category object using the database element in the SQLite Reader and returns the Category.
        /// If there is only more than one element in the reader, only the first one is converted.
        /// </summary>
        /// <param name="sqliteDataReader"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        internal static Category ToCategory(SqliteDataReader sqliteDataReader)
        {
            Category loadedCategory;
            if (sqliteDataReader.HasRows)
            {
                sqliteDataReader.Read();
                int id = sqliteDataReader.GetInt32(0);
                int categoryTypeNum = sqliteDataReader.GetInt32(1);
                //Globals.CategoryTypes categoryType = (CategoryTypes)sqliteDataReader.GetInt32(1);
                string? title = sqliteDataReader.GetString(2);
                bool isDefault = sqliteDataReader.GetBoolean(3);
                string? note = sqliteDataReader.GetString(4);
                loadedCategory = new Category(id, categoryTypeNum, title, isDefault, note);
                return loadedCategory;
            }
            else
            {
                throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Constructs Category objects using the database elements in the SQLite Reader and returns 
        /// them as  Category[]. Throws an exception if the reader contains no elements.
        /// </summary>
        /// <param name="sqliteDataReader"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        internal static Category[] ToCategories(SqliteDataReader sqliteDataReader)
        {
            List<Category> loadedCategories = new List<Category>();
            if (sqliteDataReader.HasRows)
            {
                while (sqliteDataReader.Read())
                {
                    int id = sqliteDataReader.GetInt32(0);
                    Globals.CategoryTypes categoryType = (CategoryTypes)sqliteDataReader.GetInt32(1);
                    string? title = sqliteDataReader.GetString(2);
                    bool isDefault = sqliteDataReader.GetBoolean(3);
                    string? note = sqliteDataReader.GetString(4);
                    Category loadedCategory = new Category(id, (int)categoryType, title, isDefault, note);
                    loadedCategories.Add(loadedCategory);
                }
                return loadedCategories.ToArray();
            }
            else
            {
                throw new ArgumentOutOfRangeException();
            }
        }
    }
}

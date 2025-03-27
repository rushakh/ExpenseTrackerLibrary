using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTrackerLibrary
{
    /// <summary>
    /// Contains certain enums, strings, arrays, etc. that are used by various parts of the program.
    /// </summary>
    public static class Globals
    {
        /// <summary>
        /// The various types of transactions that are aavailable.
        /// </summary>
        public enum TransactionTypes { Expense = 0, Debt = 1, Owed = 2, Earning = 3};
        /// <summary>
        /// The means by which a transaction was done (e.g., Cash or Card).
        /// </summary>
        public enum TransactionMeans { Cash, DebitCard, CreditCard, Wallet};
        /// <summary>
        /// A category is either a Main, or a Sub category.
        /// </summary>
        public enum CategoryTypes { MainCategory = 0, SubCategory = 1};
        /// <summary>
        /// The type of values that are there when importing transactions into the database.
        /// </summary>
        public enum ImportValues { TransactionType, Amount, DateAndTime, Title, Note};
        /// <summary>
        /// The location of the application's .exe file
        /// </summary>
        public static readonly string applicationPath = System.IO.Directory.GetCurrentDirectory();
        /// <summary>
        /// The culture info in use for the program.
        /// </summary>
        public static CultureInfo cultureInfo = CultureInfo.GetCultureInfo("en-GB");
        /// <summary>
        /// The default date format in use in the program.
        /// </summary>
        public static string dateTimeFormat = "dd/mm/yyyy";
        /// <summary>
        /// The names of the default main categories.
        /// </summary>
        public static readonly string[] defaultMainCategoryTitles =
                    { "Bills", "Insurance" , "Medical", "Groceries", "Skincare and Makeup", "Food and Drinks", "Clothes",
                    "Transportation", "Entertainments", "Checks", "Subscriptions", "Miscellaneous", "Unspecified", "Imported Transactions" };
        /// <summary>
        /// The names of the default sub categories.
        /// </summary>
        public static readonly string[] defaultSubCategoryTitles = { "Rent", "Electricity", "Water", "Gas", "Internet", "Rewe", "Aldi", "Penny", "Taxi", "Train/Bus" };
        /// <summary>
        /// The default keywords.
        /// </summary>
        public static readonly string[] defaultKeywords = { "Daily", "Weekly", "Monthly", "Yearly" };
    }
}

using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace ExpenseTrackerLibrary
{
    /// <summary>
    /// Abstract Class that contains methods for adding, editing, and deleting of the database table elements.
    /// </summary>
    public abstract class BaseDatabaseWriter
    {
        //private readonly string connectionString = @"Data Source=Expense_Logs.sqlite";
        private readonly string connectionString = Globals.connectionString;
        //private readonly string connectionString = @"Expense_Logs.sqlite";

        /// <summary>
        /// Constructor
        /// </summary>
        protected BaseDatabaseWriter()
        {

        }

        /// <summary>
        /// Adds a new row to the Transaction_Logs table using the given transaction properties. 
        /// Returns the value of the Id column.
        /// </summary>
        /// <param name="dateAndTime"></param>
        /// <param name="amount"></param>
        /// <param name="type"></param>
        /// <param name="isImportant"></param>
        /// <param name="keywords"></param>
        /// <param name="category"></param>
        /// <param name="title"></param>
        /// <param name="note"></param>
        /// <param name="imagePath"></param>
        /// <returns></returns>
        public int AddTransaction(DateTime dateAndTime, decimal amount, Globals.TransactionTypes type, bool isImportant, string[]? keywords, Category category, string? title, string? note, string? imagePath)
        {
            int transactionId;
            // In order for the values to be in the correct form and also not null.
            string joinedKeywords, theTitle, theNote, theImagePath;
            if (keywords is not null) { joinedKeywords = string.Join(", ", keywords); }
            else { joinedKeywords = string.Empty; }
            if (title is not null) { theTitle = title; }
            else { theTitle = string.Empty; }
            if (note is not null) { theNote = note; }
            else { theNote = string.Empty; }
            if (imagePath is not null) { theImagePath = imagePath; }
            else { theImagePath = string.Empty; }

            string commandText =
                $"INSERT INTO Transaction_Logs(DateTime, Amount, TransactionType, IsImportant, Keywords, Category, Title, Note, ImagePath) " +
                $" VALUES ('{dateAndTime.ToString(Globals.cultureInfo)}', '{amount}', '{(int)type}', '{isImportant}', '{joinedKeywords}', '{category.Id}', '{theTitle}', '{theNote}', '{theImagePath}') " +
                $" RETURNING RowId;";
            transactionId = ExecuteScalarRowId(commandText);
            return transactionId;
        }

        /// <summary>
        /// Adds a new row to the Category_Logs table using the given category properties. 
        /// Returns the value of the Id column.
        /// </summary>
        /// <param name="categoryType"></param>
        /// <param name="title"></param>
        /// <param name="isDefault"></param>
        /// <param name="note"></param>
        /// <returns></returns>
        public int AddCategory(Globals.CategoryTypes categoryType, string title, bool isDefault, string? note)
        {
            int categoryId;
            string checkedNote;
            if (note is not null) { checkedNote = note; }
            else { checkedNote = string.Empty; }

            string commandText =
                $"INSERT INTO Category_Logs(CategoryType, Title, IsDefault, Note)" +
                $" VALUES ('{(int)categoryType}', '{title}', '{isDefault}', '{checkedNote}')" +
                $" RETURNING RowId";
            categoryId = ExecuteScalarRowId(commandText);
            return categoryId;
        }

        /// <summary>
        /// Adds a new row to the Accounts_Logs table using the given accounts properties. 
        /// Returns the value of the Id column.
        /// </summary>
        /// <param name="beginning"></param>
        /// <param name="end"></param>
        /// <param name="expensesSum"></param>
        /// <param name="debtSum"></param>
        /// <param name="owedSum"></param>
        /// <param name="earningSum"></param>
        /// <returns></returns>
        public int AddAccount(DateTime beginning, DateTime end, decimal expensesSum, decimal debtSum, decimal owedSum, decimal earningSum)
        {
            int accountsId;
            string commandText =
                $"INSERT INTO Accounts_Logs(BeginningDate, EndDate, ExpensesSum, DebtSum, OwedSum, EarningSum)" +
                $" VALUES ('{beginning.ToString()}', '{end.ToString()}', '{expensesSum}', '{debtSum}', '{owedSum}', '{earningSum}')" +
                $" RETURNING RowId";
            accountsId = ExecuteScalarRowId(commandText);
            return accountsId;
        }

        /// <summary>
        /// Adds a new row to the Keywords_Logs table using the given keyword properties. 
        /// Returns the value of the Id column.
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public int AddKeyword(string keyword)
        {
            int keywordId;
            string checkedKeyword;
            if (keyword is not null) { checkedKeyword = keyword; }
            else { checkedKeyword = string.Empty; }

            string commandText =
                $"INSERT INTO Keywords_Logs(Word)" +
                $" VALUES ('{checkedKeyword}')" +
                $" RETURNING RowId";
            keywordId = ExecuteScalarRowId(commandText);
            return keywordId;
        }

        /// <summary>
        /// Updates the values of a row in the Transaction_Logs table using the updated transaction. 
        /// Returns the number of Rows that were affected (should not be more than one since Id is used 
        /// as the condition).
        /// </summary>
        /// <param name="editedTransaction"></param>
        /// <returns></returns>
        public int UpdateTransaction(Transaction editedTransaction)
        {
            int affectedRows;
            string theDateTime = editedTransaction.Date.ToString();
            decimal amount = editedTransaction.Amount;
            int type = (int)editedTransaction.TransactionType;
            int categoryId = editedTransaction.Category.Id;
            bool isImportant = editedTransaction.IsImportant;
            // In order for the values to be in the correct form and also not null.
            string checkedKeywords, theTitle, theNote, theImagePath;
            if (editedTransaction.Keywords is not null) { checkedKeywords = string.Join(", ", editedTransaction.Keywords); }
            else { checkedKeywords = string.Empty; }
            if (editedTransaction.Title is not null) { theTitle = editedTransaction.Title; }
            else { theTitle = string.Empty; }
            if (editedTransaction.Note is not null) { theNote = editedTransaction.Note; }
            else { theNote = string.Empty; }
            if (editedTransaction.ImagePath is not null) { theImagePath = editedTransaction.ImagePath; }
            else { theImagePath = string.Empty; }

            string commandText =
                $"UPDATE Transaction_Logs" +
                $" SET DateTime = '{theDateTime}', Amount = '{amount}', TransactionType = '{type}', IsImportant = '{isImportant}', Keywords = '{checkedKeywords}', Category = '{categoryId}', Title = '{theTitle}', Note = '{theNote}', ImagePath = '{theImagePath}'" +
                $" WHERE Id = '{editedTransaction.Id}';";
            affectedRows = ExecuteNonQueryCommand(commandText);
            return affectedRows;
        }

        /// <summary>
        /// Updates the values of a row in the Category_Logs table using the updated category. 
        /// Returns the number of Rows that were affected (should not be more than one since Id is used 
        /// as the condition).
        /// </summary>
        /// <param name="editedCategory"></param>
        /// <returns></returns>
        public int UpdateCategory(Category editedCategory)
        {
            int affectedRows;
            int categoryId = editedCategory.Id;
            int type = (int)editedCategory.CategoryType;
            string title = editedCategory.Title;
            bool isDefault = editedCategory.isDefaultCategory;
            string note;
            if (editedCategory.Note is not null) { note = editedCategory.Note; }
            else { note = string.Empty; }

            string commandText =
                $"UPDATE Category_Logs" +
                $" SET CategoryType = '{type}', Title = '{title}', IsDefault = '{isDefault}', Note = '{note}'" +
                $" WHERE Id = '{editedCategory.Id}';";
            affectedRows = ExecuteNonQueryCommand(commandText);
            return affectedRows;
        }

        /// <summary>
        /// Updates the values of a row in the Accounts_Logs table using the updated accounts. 
        /// Returns the number of Rows that were affected (should not be more than one since Id is used 
        /// as the condition).
        /// </summary>
        /// <param name="editedAccounts"></param>
        /// <returns></returns>
        public int UpdateAccount(Accounts editedAccounts)
        {
            int affectedRows;
            int accountsId = editedAccounts.Id;
            string beginDate = editedAccounts.Beginning.ToString();
            string endDate = editedAccounts.End.ToString();
            decimal expense = editedAccounts.ExpenseSum;
            decimal debt = editedAccounts.DebtSum;
            decimal owed = editedAccounts.OwedSum;
            decimal earning = editedAccounts.EarningSum;

            string commandText =
                $"UPDATE Accounts_Logs" +
                $" SET BeginningDate = '{beginDate}', EndDate = '{endDate}', ExpensesSum = '{expense}', DebtSum = '{debt}', OwedSum = '{owed}', EarningSum = '{earning}'" +
                $" WHERE Id = '{editedAccounts.Id}';";
            affectedRows = ExecuteNonQueryCommand(commandText);
            return affectedRows;
        }

        /// <summary>
        /// Updates the values of a row in the Keywords_Logs table using the old and new keyword. 
        /// Returns the number of Rows that were affected.
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="editedKeyword"></param>
        /// <returns></returns>
        public int UpdateKeyword(string keyword, string editedKeyword)
        {
            int affectedRows;
            string commandText =
                $"UPDATE Keywords_Logs" +
                $" SET Word = '{editedKeyword}'" +
                $" WHERE Word = '{keyword}';";
            affectedRows = ExecuteNonQueryCommand(commandText);
            return affectedRows;
        }

        /// <summary>
        /// Removes all the rows in the Transaction_Logs table and returns the number of Rows that 
        /// were affected (To be used mostly for Unit tests).
        /// </summary>
        /// <returns></returns>
        public int DeleteAllTransactions()
        {
            int affectedRows;
            string commandText =
                $"DELETE FROM Transaction_Logs";
            affectedRows = ExecuteNonQueryCommand(commandText);
            return affectedRows;
        }

        /// <summary>
        /// Removes a row from the Transaction_Logs table using Id. Returns the number of Rows that 
        /// were affected (should not be more than one since Id is used as the condition).
        /// </summary>
        /// <param name="transactionId"></param>
        /// <returns></returns>
        public int DeleteTransaction(int transactionId)
        {
            int affectedRows;
            string commandText =
                $"DELETE FROM Transaction_Logs" +
                $" WHERE Id={transactionId}";
            affectedRows = ExecuteNonQueryCommand(commandText);
            return affectedRows;
        }

        /// <summary>
        /// Removes all the rows in the Transaction_Logs table and returns the number of Rows that 
        /// were affected (To be used mostly for Unit tests).
        /// </summary>
        /// <returns></returns>
        public int DeleteAllCategories()
        {
            int affectedRows;
            string commandText =
                $"DELETE FROM Category_Logs";
            affectedRows = ExecuteNonQueryCommand(commandText);
            return affectedRows;
        }

        /// <summary>
        /// Removes a row from the Category_Logs table using title. Returns the number of Rows that 
        /// were affected.
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public int DeleteCategory(string title)
        {
            int affectedRows;
            string commandText =
                $"DELETE FROM Category_Logs" +
                $" WHERE Title = '{title}'";
            affectedRows = ExecuteNonQueryCommand(commandText);
            return affectedRows;
        }

        /// <summary>
        /// Removes a row from the Category_Logs table using Id. Returns the number of Rows that 
        /// were affected (should not be more than one since Id is used as the condition).
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public int DeleteCategory(int categoryId)
        {
            int affectedRows;
            string commandText =
                $"DELETE FROM Category_Logs" +
                $" WHERE Id = '{categoryId}'";
            affectedRows = ExecuteNonQueryCommand(commandText);
            return affectedRows;
        }

        /// <summary>
        /// Removes all the rows in the Accounts_Logs table and returns the number of Rows that 
        /// were affected (To be used mostly for Unit tests).
        /// </summary>
        /// <returns></returns>
        public int DeleteAllAccounts()
        {
            int affectedRows;
            string commandText =
                $"DELETE FROM Accounts_Logs";
            affectedRows = ExecuteNonQueryCommand(commandText);
            return affectedRows;
        }

        /// <summary>
        /// Removes a row from the Accounts_Logs table using Id. Returns the number of Rows that 
        /// were affected (should not be more than one since Id is used as the condition).
        /// </summary>
        /// <param name="accountsId"></param>
        /// <returns></returns>
        public int DeleteAccounts(int accountsId)
        {
            int affectedRows;
            string commandText =
                $"DELETE FROM Accounts_Logs" +
                $" WHERE Id = '{accountsId}'";
            affectedRows = ExecuteNonQueryCommand(commandText);
            return affectedRows;
        }

        /// <summary>
        /// Removes all the rows in the Keywords_Logs table and returns the number of Rows that 
        /// were affected (To be used mostly for Unit tests).
        /// </summary>
        /// <returns></returns>
        public int DeleteAllKeywords()
        {
            int affectedRows;
            string commandText =
                $"DELETE FROM Keywords_Logs";
            affectedRows = ExecuteNonQueryCommand(commandText);
            return affectedRows;
        }

        /// <summary>
        /// Removes a row from the Keywords_Logs table using Id. Returns the number of Rows that 
        /// were affected.
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public int DeleteKeyword(string keyword)
        {
            int affectedRows;
            string commandText =
                $"DELETE FROM Keywords_Logs" +
                $" WHERE Word = '{keyword}'";
            affectedRows = ExecuteNonQueryCommand(commandText);
            return affectedRows;
        }

        /// <summary>
        /// Executes the Non-Query command against the database and returns the number
        /// of affected rows.
        /// </summary>
        /// <param name="cmndText"></param>
        /// <returns></returns>
        private int ExecuteNonQueryCommand(string cmndText)
        {
            int affectedRows;
            using (SqliteConnection databaseConnection = new SqliteConnection(connectionString))
            {
                databaseConnection.Open();
                var databaseCommand = databaseConnection.CreateCommand();
                databaseCommand.CommandText = cmndText;
                affectedRows= databaseCommand.ExecuteNonQuery();
            }
                // **** this one is using The Sqlite-net-pcl package
                /*
                using (SQLite.SQLiteConnection databaseConnection = new SQLite.SQLiteConnection(connectionString))
                {
                    var sqliteCommand = databaseConnection.CreateCommand(cmndText);
                    affectedRows = sqliteCommand.ExecuteNonQuery();
                    databaseConnection.Close();
                }
                */
                return affectedRows;
        }

        /// <summary>
        /// Executes Scalar against the database and returns the returned Row Id (Id).
        /// </summary>
        /// <param name="cmndText"></param>
        /// <returns></returns>
        private int ExecuteScalarRowId(string cmndText)
        {
            int rowId;
            using (SqliteConnection databaseConnection = new SqliteConnection(connectionString))
            {
                databaseConnection.Open();
                var databaseCommand = databaseConnection.CreateCommand();
                databaseCommand.CommandText = cmndText;
                rowId = Convert.ToInt32(databaseCommand.ExecuteScalar());
            }

            // **** this one is using The Sqlite-net-pcl package
            /*
            using (SQLite.SQLiteConnection databaseConnection = new SQLite.SQLiteConnection(connectionString))
            {
                var sqliteCommand = databaseConnection.CreateCommand(cmndText);
                rowId = sqliteCommand.ExecuteScalar<int>();
                databaseConnection.Close();
            }
            */
            return rowId;
        }
    }

}

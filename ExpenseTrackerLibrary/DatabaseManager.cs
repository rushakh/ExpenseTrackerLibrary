using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using static ExpenseTrackerLibrary.Globals;

namespace ExpenseTrackerLibrary
{
    internal static class DatabaseManager
    {
        private static string connectionString = @"Data Source=Expense_Logs.sq";

        /// <summary>
        /// Contains methods for adding and editing of the database and its tables.
        /// </summary>
        internal static class DatabaseWriter
        {
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
            internal static int AddTransaction (DateTime dateAndTime, decimal amount, Globals.TransactionTypes type, bool isImportant, string[]? keywords, Category category, string? title, string? note, string? imagePath)
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

                using (var databaseConnection = new Microsoft.Data.Sqlite.SqliteConnection())
                {
                    databaseConnection.ConnectionString = connectionString;
                    databaseConnection.Open();
                    var sqliteCommand = databaseConnection.CreateCommand();
                    sqliteCommand.CommandText =
                        $"INSERT INTO Transaction_Logs(DateTime, Amount, TransactionType, IsImportant, Keywords, Category, Title, Note, ImagePath) " +
                        $"VALUES ('{dateAndTime.ToString()}', '{amount}', '{(int)type}', '{isImportant}', '{joinedKeywords}', '{category.Id}', '{theTitle}', '{theNote}', '{theImagePath}') " +
                        $"RETURNING RowId;";
                    transactionId = (int)sqliteCommand.ExecuteScalar();
                    databaseConnection.Close();
                }
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
            internal static int AddCategory (Globals.CategoryTypes categoryType, string title, bool isDefault, string? note)
            {
                int categoryId;
                string checkedNote;
                if (note is not null) { checkedNote = note; }
                else { checkedNote = string.Empty; }

                using (var databaseConnection = new Microsoft.Data.Sqlite.SqliteConnection())
                {
                    databaseConnection.ConnectionString = connectionString;
                    databaseConnection.Open();
                    var sqliteCommand = databaseConnection.CreateCommand();
                    sqliteCommand.CommandText =
                        $"INSERT INTO Category_Logs(CategoryType, Title, IsDefault, Note)" +
                        $"VALUES ('{(int)categoryType}', '{title}', '{isDefault}', '{checkedNote}')" +
                        $"RETURNING RowId";
                    categoryId = (int)sqliteCommand.ExecuteScalar();
                }
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
            internal static int AddAccount (DateTime beginning, DateTime end, decimal expensesSum, decimal debtSum, decimal owedSum, decimal earningSum)
            {
                int accountsId;               
                using (var databaseConnection = new Microsoft.Data.Sqlite.SqliteConnection())
                {
                    databaseConnection.ConnectionString = connectionString;
                    databaseConnection.Open();
                    var sqliteCommand = databaseConnection.CreateCommand();
                    sqliteCommand.CommandText =
                        $"INSERT INTO Accounts_Logs(BeginningDate, EndDate, ExpensesSum, DebtSum, OwedSum, EarningSum)" +
                        $"VALUES ('{beginning.ToString()}', '{end.ToString()}', '{expensesSum}', '{debtSum}', '{owedSum}', '{earningSum}')" +
                        $"RETURNING RowId";
                    accountsId = (int)sqliteCommand.ExecuteScalar();
                }
                return accountsId;
            }

            /// <summary>
            /// Adds a new row to the Keywords_Logs table using the given keyword properties. 
            /// Returns the value of the Id column.
            /// </summary>
            /// <param name="keyword"></param>
            /// <returns></returns>
            internal static int AddKeyword (string keyword)
            {
                int keywordId;
                string checkedKeyword;
                if (keyword is not null) { checkedKeyword = keyword; }
                else { checkedKeyword = string.Empty; }
                    using (var databaseConnection = new Microsoft.Data.Sqlite.SqliteConnection())
                    {
                        databaseConnection.ConnectionString = connectionString;
                        databaseConnection.Open();
                        var sqliteCommand = databaseConnection.CreateCommand();
                        sqliteCommand.CommandText =
                            $"INSERT INTO Accounts_Logs(Word)" +
                            $"VALUES ('{checkedKeyword}')" +
                            $"RETURNING RowId";
                    keywordId = (int)sqliteCommand.ExecuteScalar();
                    }
                return keywordId;
            }

            /// <summary>
            /// Updates the values of a row in the Transaction_Logs table using the updated transaction. 
            /// Returns the number of Rows that were affected (should not be more than one since Id is used 
            /// as the condition).
            /// </summary>
            /// <param name="editedTransaction"></param>
            /// <returns></returns>
            internal static int UpdateTransaction (Transaction editedTransaction)
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

                using (var databaseConnection = new Microsoft.Data.Sqlite.SqliteConnection())
                {
                    databaseConnection.ConnectionString = connectionString;
                    databaseConnection.Open();
                    var sqliteCommand = databaseConnection.CreateCommand();
                    sqliteCommand.CommandText =
                        $"UPDATE Transaction_Logs" +
                        $"SET DateTime = '{theDateTime}', Amount = '{amount}', TransactionType = '{type}', IsImportant = '{isImportant}', Keywords = '{checkedKeywords}', Category = '{categoryId}', Title = '{theTitle}', Note = '{theNote}', ImagePath = '{theImagePath}')" +
                        $"WHERE Id = '{editedTransaction.Id}';";
                    affectedRows = sqliteCommand.ExecuteNonQuery();
                    databaseConnection.Close();
                }
                return affectedRows;
            }

            /// <summary>
            /// Updates the values of a row in the Category_Logs table using the updated category. 
            /// Returns the number of Rows that were affected (should not be more than one since Id is used 
            /// as the condition).
            /// </summary>
            /// <param name="editedCategory"></param>
            /// <returns></returns>
            internal static int UpdateCategory (Category editedCategory)
            {
                int affectedRows;
                int categoryId = editedCategory.Id;
                int type = (int)editedCategory.CategoryType;
                string title = editedCategory.Title;
                bool isDefault = editedCategory.isDefaultCategory;
                string note;
                if (editedCategory.Note is not null) { note = editedCategory.Note; }
                else { note = string.Empty; }

                using (var databaseConnection = new Microsoft.Data.Sqlite.SqliteConnection())
                {
                    databaseConnection.ConnectionString = connectionString;
                    databaseConnection.Open();
                    var sqliteCommand = databaseConnection.CreateCommand();
                    sqliteCommand.CommandText =
                        $"UPDATE Category_Logs" +
                        $"SET CategoryType = '{categoryId}', Title = '{title}', IsDefault = '{isDefault}', Note = '{note}'" +
                        $"WHERE Id = '{editedCategory.Id}';";
                    affectedRows = sqliteCommand.ExecuteNonQuery();
                    databaseConnection.Close();
                }
                return affectedRows;
            }

            /// <summary>
            /// Updates the values of a row in the Accounts_Logs table using the updated accounts. 
            /// Returns the number of Rows that were affected (should not be more than one since Id is used 
            /// as the condition).
            /// </summary>
            /// <param name="editedAccounts"></param>
            /// <returns></returns>
            internal static int UpdateAccount(Accounts editedAccounts)
            {
                int affectedRows;
                int accountsId = editedAccounts.Id;
                string beginDate = editedAccounts.Beginning.ToString();
                string endDate = editedAccounts.End.ToString();
                decimal expense = editedAccounts.ExpenseSum;
                decimal debt = editedAccounts.DebtSum;
                decimal owed = editedAccounts.OwedSum;
                decimal earning = editedAccounts.EarningSum;

                using (var databaseConnection = new Microsoft.Data.Sqlite.SqliteConnection())
                {
                    databaseConnection.ConnectionString = connectionString;
                    databaseConnection.Open();
                    var sqliteCommand = databaseConnection.CreateCommand();
                    sqliteCommand.CommandText =
                        $"UPDATE Accounts_Logs" +
                        $"SET BeginningDate = '{beginDate}', EndDate = '{endDate}', ExpensesSum = '{expense}', DebtSum = '{debt}', OwedSum = '{owed}', EarningSum = '{earning}'" +
                        $"WHERE Id = '{editedAccounts.Id}';";
                    affectedRows = sqliteCommand.ExecuteNonQuery();
                    databaseConnection.Close();
                }
                return affectedRows;
            }

            /// <summary>
            /// Updates the values of a row in the Keywords_Logs table using the old and new keyword. 
            /// Returns the number of Rows that were affected.
            /// </summary>
            /// <param name="keyword"></param>
            /// <param name="editedKeyword"></param>
            /// <returns></returns>
            internal static int UpdateKeyword(string keyword, string editedKeyword)
            {
                int affectedRows;
                using (var databaseConnection = new Microsoft.Data.Sqlite.SqliteConnection())
                {
                    databaseConnection.ConnectionString = connectionString;
                    databaseConnection.Open();
                    var sqliteCommand = databaseConnection.CreateCommand();
                    sqliteCommand.CommandText =
                        $"UPDATE Keywords_Logs" +
                        $"SET Word = '{editedKeyword}'" +
                        $"WHERE Word = '{keyword}';";
                    affectedRows = sqliteCommand.ExecuteNonQuery();
                    databaseConnection.Close();
                }
                return affectedRows;
            }

            /// <summary>
            /// Removes a row from the Transaction_Logs table using Id. Returns the number of Rows that 
            /// were affected (should not be more than one since Id is used as the condition).
            /// </summary>
            /// <param name="transactionId"></param>
            /// <returns></returns>
            internal static int DeleteTransaction (int transactionId)
            {
                int affectedRows;
                using (var databaseConnection = new Microsoft.Data.Sqlite.SqliteConnection())
                {
                    databaseConnection.ConnectionString = connectionString;
                    databaseConnection.Open();
                    var sqliteCommand = databaseConnection.CreateCommand();
                    sqliteCommand.CommandText =
                        $"DELETE FROM Transaction_Logs" +
                        $"WHERE Id = '{transactionId}'";
                    affectedRows = sqliteCommand.ExecuteNonQuery();
                    databaseConnection.Close();
                }
                return affectedRows;
            }

            /// <summary>
            /// Removes a row from the Category_Logs table using Id. Returns the number of Rows that 
            /// were affected (should not be more than one since Id is used as the condition).
            /// </summary>
            /// <param name="categoryId"></param>
            /// <returns></returns>
            internal static int DeleteCategory(int categoryId)
            {
                int affectedRows;
                using (var databaseConnection = new Microsoft.Data.Sqlite.SqliteConnection())
                {
                    databaseConnection.ConnectionString = connectionString;
                    databaseConnection.Open();
                    var sqliteCommand = databaseConnection.CreateCommand();
                    sqliteCommand.CommandText =
                        $"DELETE FROM Category_Logs" +
                        $"WHERE Id = '{categoryId}'";
                    affectedRows = sqliteCommand.ExecuteNonQuery();
                    databaseConnection.Close();
                }
                return affectedRows;
            }

            /// <summary>
            /// Removes a row from the Accounts_Logs table using Id. Returns the number of Rows that 
            /// were affected (should not be more than one since Id is used as the condition).
            /// </summary>
            /// <param name="accountsId"></param>
            /// <returns></returns>
            internal static int DeleteAccounts (int accountsId)
            {
                int affectedRows;
                using (var databaseConnection = new Microsoft.Data.Sqlite.SqliteConnection())
                {
                    databaseConnection.ConnectionString = connectionString;
                    databaseConnection.Open();
                    var sqliteCommand = databaseConnection.CreateCommand();
                    sqliteCommand.CommandText =
                        $"DELETE FROM Accounts_Logs" +
                        $"WHERE Id = '{accountsId}'";
                    affectedRows = sqliteCommand.ExecuteNonQuery();
                    databaseConnection.Close();
                }
                return affectedRows;
            }

            /// <summary>
            /// Removes a row from the Keywords_Logs table using Id. Returns the number of Rows that 
            /// were affected.
            /// </summary>
            /// <param name="keyword"></param>
            /// <returns></returns>
            internal static int DeleteKeyword (string keyword)
            {
                int affectedRows;
                using (var databaseConnection = new Microsoft.Data.Sqlite.SqliteConnection())
                {
                    databaseConnection.ConnectionString = connectionString;
                    databaseConnection.Open();
                    var sqliteCommand = databaseConnection.CreateCommand();
                    sqliteCommand.CommandText =
                        $"DELETE FROM Keywords_Logs" +
                        $"WHERE Word = '{keyword}'";
                    affectedRows = sqliteCommand.ExecuteNonQuery();
                    databaseConnection.Close();
                }
                return affectedRows;
            }
        }

        /// <summary>
        /// Contains methods for reading from the database.
        /// </summary>
        internal static class DatabaseReader
        {
            /// <summary>
            /// Checks if the database has any categories.
            /// </summary>
            /// <returns></returns>
            internal static bool HasCategories()
            {
                bool hasCategories;
                using (var databaseConnection = new Microsoft.Data.Sqlite.SqliteConnection())
                {
                    databaseConnection.ConnectionString = connectionString;
                    databaseConnection.Open();
                    var databaseQuery = databaseConnection.CreateCommand();
                    databaseQuery.CommandText = $"SELECT * FROM Category_Logs";
                    var filteredDatabaseReader = databaseQuery.ExecuteReader();
                    if (filteredDatabaseReader.HasRows)
                    {
                        hasCategories = true;
                    }
                    else
                    {
                        hasCategories = false;
                    }
                }
                return hasCategories;
            }

            /// <summary>
            /// Loads and returns all the transactions in the database as array.
            /// </summary>
            /// <returns></returns>
            internal static Transaction[]? GetAllTransactions ()
            {
                Transaction[] foundTransactions;
                using (var databaseConnection = new Microsoft.Data.Sqlite.SqliteConnection())
                {
                    databaseConnection.ConnectionString = connectionString;
                    databaseConnection.Open();
                    var databaseQuery = databaseConnection.CreateCommand();
                    databaseQuery.CommandText = $"SELECT * FROM Transaction_Logs";
                    var filteredDatabaseReader = databaseQuery.ExecuteReader();
                    if (filteredDatabaseReader.HasRows)
                    {
                        foundTransactions = SqlToTransaction.SQLiteReaderToTransactions(filteredDatabaseReader);
                        return foundTransactions;
                    }
                    else
                    {
                        return null;
                    }
                }
            }

            /// <summary>
            /// Finds, loads, and returns the transaction with the specified transaction Id. Returns null
            /// if no such transaction is found.
            /// </summary>
            /// <param name="transactionId"></param>
            /// <returns></returns>
            internal static Transaction? GetTransaction (int transactionId)
            {
                Transaction foundTransaction;
                using (var databaseConnection = new Microsoft.Data.Sqlite.SqliteConnection())
                {
                    databaseConnection.ConnectionString = connectionString;
                    databaseConnection.Open();
                    var databaseQuery = databaseConnection.CreateCommand();
                    databaseQuery.CommandText = $"SELECT Id, FROM Transaction_Logs, WHERE Id = '{transactionId}'";
                    var filteredDatabaseReader = databaseQuery.ExecuteReader();
                    if (filteredDatabaseReader.HasRows)
                    {
                        foundTransaction = SqlToTransaction.SQLiteReaderToTransaction(filteredDatabaseReader);
                        return foundTransaction;
                    }
                    else
                    {
                        return null;
                    }
                }
            }

            /// <summary>
            /// Returns a Transaction[] with transactions that took place between the specified dates.
            /// Returns null if no transaction meets the date and time criteria. The array is Ascending
            /// by default (from oldest to the most recent).
            /// </summary>
            /// <param name="fromDateTime"></param>
            /// <param name="untilDateTime"></param>
            /// <returns></returns>
            internal static Transaction[]? GetTransactions (DateTime fromDateTime, DateTime untilDateTime)
            {
                var allTransactions = GetAllTransactions();
                var foundTransactions = from transaction in allTransactions
                                        where transaction.Date >= fromDateTime
                                        where transaction.Date <= untilDateTime
                                        orderby transaction.Date ascending
                                        select transaction;
                if (foundTransactions.Any())
                {
                    return foundTransactions.ToArray();
                }
                else
                {
                    return null;
                }                    
            }

            /// <summary>
            /// Loads and returns the transactions that have transaction Id s between the specified numbers.
            /// </summary>
            /// <param name="fromId"></param>
            /// <param name="untilId"></param>
            /// <returns></returns>
            internal static Transaction[]? GetTransactions(int fromId, int untilId)
            {
                Transaction[] foundTransactions;
                using (var databaseConnection = new Microsoft.Data.Sqlite.SqliteConnection())
                {
                    databaseConnection.ConnectionString = connectionString;
                    databaseConnection.Open();
                    var databaseQuery = databaseConnection.CreateCommand();
                    databaseQuery.CommandText = $"SELECT Id, FROM Transaction_Logs, WHERE Id BETWEEN '{fromId}' AND '{untilId}'";
                    var filteredDatabaseReader = databaseQuery.ExecuteReader();
                    if (filteredDatabaseReader.HasRows)
                    {
                        foundTransactions = SqlToTransaction.SQLiteReaderToTransactions(filteredDatabaseReader);
                        return foundTransactions;
                    }
                    else
                    {
                        return null;
                    }
                }
            }

            /// <summary>
            /// Finds, loads, and returns a Transaction[] of transactions that belong to the specified Category.
            /// </summary>
            /// <param name="chosenCategory"></param>
            /// <returns></returns>
            internal static Transaction[]? GetTransactions (Category chosenCategory)
            {
                Transaction[] foundTransactions;
                using (var databaseConnection = new Microsoft.Data.Sqlite.SqliteConnection())
                {
                    databaseConnection.ConnectionString = connectionString;
                    databaseConnection.Open();
                    var databaseQuery = databaseConnection.CreateCommand();
                    databaseQuery.CommandText = $"SELECT Category, FROM Transaction_Logs, WHERE Category = '{chosenCategory.Id}'";
                    var filteredDatabaseReader = databaseQuery.ExecuteReader();
                    if (filteredDatabaseReader.HasRows)
                    {
                        foundTransactions = SqlToTransaction.SQLiteReaderToTransactions(filteredDatabaseReader);
                        return foundTransactions;
                    }
                    else
                    {
                        return null;
                    }
                }
            }

            /// <summary>
            /// Finds, loads, and returns a Transaction[] that contains the specified string as a Keyword (case sensitive).
            /// </summary>
            /// <param name="keyword"></param>
            /// <returns></returns>
            internal static Transaction[]? GetTransactions (string keyword)
            {
                // It's better for now to search for the keyword in the loaded transactions to prevent
                // problems occuring, since all keywords are in one column and seperated by a single
                // character. *** might change this if I find a better solution or a better design.
                var allTransactions = GetAllTransactions();
                var foundTransactions = from transaction in allTransactions
                                        where transaction.HasKeywords
                                        where transaction.Keywords.Contains (keyword)
                                        orderby transaction.Date descending
                                        select transaction;
                if (foundTransactions.Any())
                {
                    return foundTransactions.ToArray();
                }
                else
                {
                    return null;
                }
            }

            /// <summary>
            /// Finds, loads, returns a Transaction[] that are of the specified transaction type.
            /// </summary>
            /// <param name="transactionType"></param>
            /// <returns></returns>
            internal static Transaction[]? GetTransactions (Globals.TransactionTypes transactionType)
            {
                int typeNum = (int)transactionType;
                Transaction[] foundTransactions;
                using (var databaseConnection = new Microsoft.Data.Sqlite.SqliteConnection())
                {
                    databaseConnection.ConnectionString = connectionString;
                    databaseConnection.Open();
                    var databaseQuery = databaseConnection.CreateCommand();
                    databaseQuery.CommandText = $"SELECT TransactionType, FROM Transaction_Logs, WHERE TransactionType = '{typeNum}'";
                    var filteredDatabaseReader = databaseQuery.ExecuteReader();
                    if (filteredDatabaseReader.HasRows)
                    {
                        foundTransactions = SqlToTransaction.SQLiteReaderToTransactions(filteredDatabaseReader);
                        return foundTransactions;
                    }
                    else
                    {
                        return null;
                    }
                }
            }

            /// <summary>
            /// Finds, loads, and returns a Transaction[] with transactions that have the specified Amount.
            /// </summary>
            /// <param name="amount"></param>
            /// <returns></returns>
            internal static Transaction[]? GetTransactions (decimal amount)
            {
                // **** DOUBLE CHECK I'm not sure about the DECIMAL and float in here.
                // **** MIGHT BE BETTER to change the command text to LIKE to find the pattern instead
                // of the specific number.
                Transaction[] foundTransactions;
                using (var databaseConnection = new Microsoft.Data.Sqlite.SqliteConnection())
                {
                    databaseConnection.ConnectionString = connectionString;
                    databaseConnection.Open();
                    var databaseQuery = databaseConnection.CreateCommand();
                    databaseQuery.CommandText = $"SELECT Amount, FROM Transaction_Logs, WHERE Amount = '{amount}'";
                    var filteredDatabaseReader = databaseQuery.ExecuteReader();
                    if (filteredDatabaseReader.HasRows)
                    {
                        foundTransactions = SqlToTransaction.SQLiteReaderToTransactions(filteredDatabaseReader);
                        return foundTransactions;
                    }
                    else
                    {
                        return null;
                    }
                }
            }

            /// <summary>
            /// Finds, loads and returns a Transaction[] that have the specified string as Title or Note. By 
            /// default more than just Exact matches are considered and returned. 
            /// </summary>
            /// <param name="titleOrNote"></param>
            /// <param name="exactMatch"></param>
            /// <returns></returns>
            internal static Transaction[]? GetTransactions (string titleOrNote, bool exactMatch = false)
            {
                Transaction[] foundTransactions;
                using (var databaseConnection = new Microsoft.Data.Sqlite.SqliteConnection())
                {
                    databaseConnection.ConnectionString = connectionString;
                    databaseConnection.Open();
                    var databaseQuery = databaseConnection.CreateCommand();
                    string commandText;
                    if (exactMatch == true)
                    {
                        commandText = $"SELECT Title, Note, FROM Transaction_Logs, WHERE Title = '{titleOrNote}' OR Note = '{titleOrNote}'";
                    }
                    else
                    {
                        commandText = $"SELECT Title, Note, FROM Transaction_Logs, WHERE Title = '{titleOrNote}' OR Title LIKE '{titleOrNote}' OR Note = '{titleOrNote}' OR Note LIKE '{titleOrNote}'";
                    }
                    databaseQuery.CommandText = commandText;
                    var filteredDatabaseReader = databaseQuery.ExecuteReader();
                    if (filteredDatabaseReader.HasRows)
                    {
                        foundTransactions = SqlToTransaction.SQLiteReaderToTransactions(filteredDatabaseReader);
                        return foundTransactions;
                    }
                    else
                    {
                        return null;
                    }
                }
            }

            /// <summary>
            /// Finds and returns a category using its Id. returns null if it doesn't exist.
            /// </summary>
            /// <param name="categoryId"></param>
            /// <returns></returns>
            internal static Category? GetCategory (int categoryId)
            {
                Category foundCategory;
                using (var databaseConnection = new Microsoft.Data.Sqlite.SqliteConnection())
                {
                    databaseConnection.ConnectionString = connectionString;
                    databaseConnection.Open();
                    var databaseQuery = databaseConnection.CreateCommand();
                    databaseQuery.CommandText = $"SELECT Id, FROM Category_Logs, WHERE Id = '{categoryId}'";
                    var filteredDatabaseReader = databaseQuery.ExecuteReader();
                    if (filteredDatabaseReader.HasRows)
                    {
                        foundCategory = SqlToCategory.SQLiteReaderToCategory(filteredDatabaseReader);
                        return foundCategory;
                    }
                    else
                    {
                        return null;
                    }
                }
            }

        }      
    }
}

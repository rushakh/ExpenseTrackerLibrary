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
            internal static int AddTransaction (DateTime dateAndTime, float amount, Globals.TransactionTypes type, bool isImportant, string[]? keywords, Category category, string? title, string? note, string? imagePath)
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
        
            internal static int AddCategory ()
            {
                // *** NOT WRITTEN
                return 0;
            }

            internal static int AddAccount ()
            {
                // *** Not Written
                return 0;
            }

            internal static int AddKeyword ()
            {
                // *** Not Written
                return 0;
            }

            internal static bool UpdateTransaction ()
            {
                // *** Not Written
                return false;
            }

            internal static bool UpdateCategory ()
            {
                // *** Not Written
                return false;
            }

            internal static bool UpdateAccount()
            {
                // *** Not Written
                return false;
            }

            internal static bool UpdateKeyword()
            {
                // *** Not Written
                return false;
            }

            internal static bool DeleteTransaction ()
            {
                // *** Not Written
                return false;
            }

            internal static bool DeleteCategory()
            {
                // *** Not Written
                return false;
            }

            internal static bool DeleteAccount ()
            {
                // *** Not Written
                return false;
            }

            internal static bool DeleteKeyword ()
            {
                // *** Not Written
                return false;
            }

        }

        /// <summary>
        /// Contains methods for reading from the database.
        /// </summary>
        internal static class DatabaseReader
        {
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
            internal static Transaction[]? GetTransactions (float amount)
            {
                // **** DOUBLE CHECK I'm not sure about the DECIMAL and float in here.
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

        /// <summary>
        /// Returns an integar that can be used as an identification number for a new transaction.
        /// </summary>
        /// <returns></returns>
        internal static int GetNewCategoryId ()
        {
            int lastId = 0;
            // Check the last database Entry id and then + 1
            int newId = lastId + 1;
            // OR Directly add the new entry to the database and Then check the id that was determined for it
            // and use that. 2nd way is safer, but has extra steps.
            return newId;
        }

    }
}

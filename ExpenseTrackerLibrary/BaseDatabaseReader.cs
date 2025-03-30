using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTrackerLibrary
{
    /// <summary>
    /// Abstract class that contains methods for reading from the database and checking the existance of certain elements.
    /// </summary>
     public abstract class BaseDatabaseReader
    {
        private readonly string connectionString = @"Data Source=Expense_Logs.sqlite";

        /// <summary>
        /// Checks if the database has any categories.
        /// </summary>
        /// <returns></returns>
        public bool HasCategories()
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
        /// Loads and returns all the transactions in the database as an array.
        /// </summary>
        /// <returns></returns>
        public Transaction[]? GetAllTransactions()
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
                    foundTransactions = ConvertSQLiteReader.ToTransactions(filteredDatabaseReader);
                    return foundTransactions;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Loads and returns all the Categories in the database as an array. Returns null if there 
        /// are no categories in the database.
        /// </summary>
        /// <returns></returns>
        public Category[]? GetAllCategories()
        {
            Category[] foundCategories;
            using (var databaseConnection = new Microsoft.Data.Sqlite.SqliteConnection())
            {
                databaseConnection.ConnectionString = connectionString;
                databaseConnection.Open();
                var databaseQuery = databaseConnection.CreateCommand();
                databaseQuery.CommandText = $"SELECT * FROM Category_Logs";
                var filteredDatabaseReader = databaseQuery.ExecuteReader();
                if (filteredDatabaseReader.HasRows)
                {
                    foundCategories = ConvertSQLiteReader.ToCategories(filteredDatabaseReader);
                    return foundCategories;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Loads and return all the keywords in the database as an arry.
        /// </summary>
        /// <returns></returns>
        public String[]? GetAllKeywords()
        {
            List<string> foundKeywords = new List<string>();
            using (var databaseConnection = new Microsoft.Data.Sqlite.SqliteConnection())
            {
                databaseConnection.ConnectionString = connectionString;
                databaseConnection.Open();
                var databaseQuery = databaseConnection.CreateCommand();
                databaseQuery.CommandText = $"SELECT * FROM Keywords_Logs";
                var filteredDatabaseReader = databaseQuery.ExecuteReader();
                if (filteredDatabaseReader.HasRows)
                {
                    while (filteredDatabaseReader.Read())
                    {
                        string keyword = filteredDatabaseReader.GetString(1);
                        foundKeywords.Add(keyword);
                    }
                    return foundKeywords.ToArray();
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Finds and returns a category using its Id. Throws an exception if 
        /// a category with that Id doesn't exist.
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public Category GetCategory(int categoryId)
        {
            Category foundCategory;
            using (var databaseConnection = new Microsoft.Data.Sqlite.SqliteConnection())
            {
                databaseConnection.ConnectionString = connectionString;
                databaseConnection.Open();
                var databaseQuery = databaseConnection.CreateCommand();
                databaseQuery.CommandText = $"SELECT * FROM Category_Logs WHERE Id = '{categoryId}'";
                var filteredDatabaseReader = databaseQuery.ExecuteReader();
                if (filteredDatabaseReader.HasRows)
                {
                    foundCategory = ConvertSQLiteReader.ToCategory(filteredDatabaseReader);
                    return foundCategory;
                }
                else
                {
                    throw new ArgumentException("categoryId", "No Category with this Id exists.");
                }
            }
        }

        /// <summary>
        /// Finds, loads, and returns the transaction with the specified transaction Id. Throws an
        /// exception if it doesn't exist.
        /// </summary>
        /// <param name="transactionId"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public Transaction GetTransaction(int transactionId)
        {
            Transaction foundTransaction;
            using (var databaseConnection = new Microsoft.Data.Sqlite.SqliteConnection())
            {
                databaseConnection.ConnectionString = connectionString;
                databaseConnection.Open();
                var databaseQuery = databaseConnection.CreateCommand();
                databaseQuery.CommandText = $"SELECT * FROM Transaction_Logs WHERE Id = '{transactionId}'";
                var filteredDatabaseReader = databaseQuery.ExecuteReader();
                if (filteredDatabaseReader.HasRows)
                {
                    foundTransaction = ConvertSQLiteReader.ToTransaction(filteredDatabaseReader);
                    return foundTransaction;
                }
                else
                {
                    throw new ArgumentException("transactionId", "No Transaction with this Id exists.");
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
        public Transaction[]? GetTransactions(DateTime fromDateTime, DateTime untilDateTime)
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
        /// Throws an exception if no transaction exists between the two.
        /// </summary>
        /// <param name="fromId"></param>
        /// <param name="untilId"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public Transaction[] GetTransactions(int fromId, int untilId)
        {
            Transaction[] foundTransactions;
            using (var databaseConnection = new Microsoft.Data.Sqlite.SqliteConnection())
            {
                databaseConnection.ConnectionString = connectionString;
                databaseConnection.Open();
                var databaseQuery = databaseConnection.CreateCommand();
                databaseQuery.CommandText = $"SELECT * FROM Transaction_Logs WHERE Id BETWEEN '{fromId}' AND '{untilId}'";
                var filteredDatabaseReader = databaseQuery.ExecuteReader();
                if (filteredDatabaseReader.HasRows)
                {
                    foundTransactions = ConvertSQLiteReader.ToTransactions(filteredDatabaseReader);
                    return foundTransactions;
                }
                else
                {
                    throw new ArgumentException("fromId / untilId", "No Transaction with such Id exists.");
                }
            }
        }

        /// <summary>
        /// Finds, loads, and returns a Transaction[] of transactions that belong to the specified Category.
        /// </summary>
        /// <param name="chosenCategory"></param>
        /// <returns></returns>
        public Transaction[]? GetTransactions(Category chosenCategory)
        {
            Transaction[] foundTransactions;
            using (var databaseConnection = new Microsoft.Data.Sqlite.SqliteConnection())
            {
                databaseConnection.ConnectionString = connectionString;
                databaseConnection.Open();
                var databaseQuery = databaseConnection.CreateCommand();
                databaseQuery.CommandText = 
                    $"SELECT * FROM Transaction_Logs" +
                    $" WHERE Category = '{chosenCategory.Id}'";
                var filteredDatabaseReader = databaseQuery.ExecuteReader();
                if (filteredDatabaseReader.HasRows)
                {
                    foundTransactions = ConvertSQLiteReader.ToTransactions(filteredDatabaseReader);
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
        public Transaction[]? GetTransactions(string keyword)
        {
            // It's better for now to search for the keyword in the loaded transactions to prevent
            // problems occuring, since all keywords are in one column and seperated by a single
            // character. *** might change this if I find a better solution or a better design.
            var allTransactions = GetAllTransactions();
            if (allTransactions is not null && allTransactions.Any())
            {
                var foundTransactions = from transaction in allTransactions
                                        where transaction.HasKeywords
                                        where transaction.Keywords!.Contains(keyword)
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
        public Transaction[]? GetTransactions(Globals.TransactionTypes transactionType)
        {
            int typeNum = (int)transactionType;
            Transaction[] foundTransactions;
            using (var databaseConnection = new Microsoft.Data.Sqlite.SqliteConnection())
            {
                databaseConnection.ConnectionString = connectionString;
                databaseConnection.Open();
                var databaseQuery = databaseConnection.CreateCommand();
                databaseQuery.CommandText = $"SELECT *" +
                    $" FROM Transaction_Logs" +
                    $" WHERE TransactionType = '{typeNum}'";
                var filteredDatabaseReader = databaseQuery.ExecuteReader();
                if (filteredDatabaseReader.HasRows)
                {
                    foundTransactions = ConvertSQLiteReader.ToTransactions(filteredDatabaseReader);
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
        public Transaction[]? GetTransactions(decimal amount)
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
                databaseQuery.CommandText = $"SELECT *" +
                    $" FROM Transaction_Logs" +
                    $" WHERE Amount = '{amount}'";
                var filteredDatabaseReader = databaseQuery.ExecuteReader();
                if (filteredDatabaseReader.HasRows)
                {
                    foundTransactions = ConvertSQLiteReader.ToTransactions(filteredDatabaseReader);
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
        public Transaction[]? GetTransactions(string titleOrNote, bool exactMatch = false)
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
                    commandText = $"SELECT *" +
                        $" FROM Transaction_Logs" +
                        $" WHERE Title = '{titleOrNote}' OR Note = '{titleOrNote}'";
                }
                else
                {
                    commandText = $"SELECT *" +
                        $" FROM Transaction_Logs" +
                        $" WHERE Title LIKE '%{titleOrNote}%' OR Note LIKE '%{titleOrNote}%'";
                    //$" WHERE Title = '{titleOrNote}' OR Title LIKE '{titleOrNote}' OR Note = '{titleOrNote}' OR Note LIKE '{titleOrNote}'";
                }
                databaseQuery.CommandText = commandText;
                var filteredDatabaseReader = databaseQuery.ExecuteReader();
                if (filteredDatabaseReader.HasRows)
                {
                    foundTransactions = ConvertSQLiteReader.ToTransactions(filteredDatabaseReader);
                    return foundTransactions;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Checks if a Category with that name already exists. Returns true if it does, otherwise false.
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public bool CategoryExists(string title)
        {
            if (title is null || title == string.Empty) { throw new ArgumentException(); }
            bool exists;
            using (var databaseConnection = new Microsoft.Data.Sqlite.SqliteConnection())
            {
                databaseConnection.ConnectionString = connectionString;
                databaseConnection.Open();
                var databaseQuery = databaseConnection.CreateCommand();
                databaseQuery.CommandText = $"SELECT Title" +
                    $" FROM Category_Logs" +
                    $" WHERE Title = '{title}'";
                var filteredDatabaseReader = databaseQuery.ExecuteReader();
                if (filteredDatabaseReader.HasRows) { exists = true; }
                else { exists = false; }
            }
            return exists;
        }

        /// <summary>
        /// Checks if a keyword already exists. Returns true if it does, otherwise false.
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public bool KeywordExists(string word)
        {
            if (word is null || word == string.Empty) { throw new ArgumentException(); }
            bool exists;
            using (var databaseConnection = new Microsoft.Data.Sqlite.SqliteConnection())
            {
                databaseConnection.ConnectionString = connectionString;
                databaseConnection.Open();
                var databaseQuery = databaseConnection.CreateCommand();
                databaseQuery.CommandText = $"SELECT Word" +
                    $" FROM Keywords_Logs" +
                    $" WHERE Word = '{word}'";
                var filteredDatabaseReader = databaseQuery.ExecuteReader();
                if (filteredDatabaseReader.HasRows) { exists = true; }
                else { exists = false; }
            }
            return exists;
        }
    }
}

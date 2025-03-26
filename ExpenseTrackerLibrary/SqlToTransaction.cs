using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ExpenseTrackerLibrary.Globals;

namespace ExpenseTrackerLibrary
{
    /// <summary>
    /// A class that contains tools for converting the SQLiteReader of the database to objects of type Transaction.
    /// </summary>
    internal static class SqlToTransaction
    {
        /// <summary>
        /// Constructs a Transaction object using the database element in the SQLite Reader and returns Transaction.
        /// Converts only the first element in the reader if there are more than one.
        /// </summary>
        /// <param name="sqliteDataReader"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        internal static Transaction SQLiteReaderToTransaction (SqliteDataReader sqliteDataReader)
        {           
            if (sqliteDataReader.HasRows)
            {
                Transaction loadedTransaction;
                sqliteDataReader.Read();
                int id = sqliteDataReader.GetInt32(0);
                DateTime dateTime = sqliteDataReader.GetDateTime(1);
                decimal amount = (decimal)sqliteDataReader.GetDouble(2);
                Globals.TransactionTypes transactionType = (TransactionTypes)sqliteDataReader.GetInt32(3);
                bool isImportant = sqliteDataReader.GetBoolean(4);
                string[] keywords = GetKeywords(sqliteDataReader.GetString(5));
                Category category = GetCategory(sqliteDataReader.GetInt32(6));
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
        internal static Transaction[] SQLiteReaderToTransactions (SqliteDataReader sqliteDataReader)
        {
            if (sqliteDataReader.HasRows)
            {
                List<Transaction> transactions = new List<Transaction>();
                while (sqliteDataReader.Read())
                {
                    int id = sqliteDataReader.GetInt32(0);
                    DateTime dateTime = sqliteDataReader.GetDateTime(1);
                    decimal amount = (decimal)sqliteDataReader.GetDouble(2);
                    Globals.TransactionTypes transactionType = (TransactionTypes)sqliteDataReader.GetInt32(3);
                    bool isImportant = sqliteDataReader.GetBoolean(4);
                    string[] keywords = GetKeywords(sqliteDataReader.GetString(5));
                    Category category = GetCategory(sqliteDataReader.GetInt32(6));
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
        /// Returns the Category with the specified category Id.
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        private static Category GetCategory (int categoryId)
        {
            var tempCategory = DatabaseManager.DatabaseReader.GetCategory(categoryId);
            return tempCategory;
        }

        /// <summary>
        /// Returns a string[] populated by seperating the aggregated keywords in the given string.
        /// </summary>
        /// <param name="keywords"></param>
        /// <returns></returns>
        private static string[] GetKeywords (string keywords)
        {
            string[] tempKeywords = keywords.Split(", ", StringSplitOptions.TrimEntries);
            return tempKeywords;
        }

        private static DateTime GetDateTime (string SQLiteDateTime)
        {
            // *** LATER
            return DateTime.MinValue;
        }


    }
}

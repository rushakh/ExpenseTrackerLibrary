using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ExpenseTrackerLibrary.Globals;

namespace ExpenseTrackerLibrary
{
    internal static class SqlToTransaction
    {
        /// <summary>
        /// Constructs Transaction objects using the database elements in the SQLite Reader and returns Transaction[].
        /// </summary>
        /// <param name="sqliteDataReader"></param>
        /// <returns></returns>
        internal static Transaction[] SQLiteReaderToTransaction(SqliteDataReader sqliteDataReader)
        {            
            List<Transaction> transactions = new List<Transaction>();
            while (sqliteDataReader.Read())
            {
                int id = sqliteDataReader.GetInt32(0);
                DateTime dateTime = sqliteDataReader.GetDateTime(1);
                float amount = (float) sqliteDataReader.GetDouble(2);
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
            string[] tempKeywords = keywords.Split(',', StringSplitOptions.TrimEntries);
            return tempKeywords;
        }

        private static DateTime GetDateTime (string SQLiteDateTime)
        {
            return DateTime.MinValue;
        }


    }
}

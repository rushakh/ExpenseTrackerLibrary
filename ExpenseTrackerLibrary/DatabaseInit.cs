using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace ExpenseTrackerLibrary
{
    /// <summary>
    /// Initializes the database and creates the various tables that are required.
    /// </summary>
    public static class DatabaseInitialization
    {
        private static string connectionString = @"Data Source=Expense_Logs.sqlite";
        private static string databaseName = "Expense_Logs.sqlite";

        /// <summary>
        /// Contains the methods for the creation of the database tables.
        /// </summary>
        private static class DatabaseTables
        {
            /// <summary>
            /// Creates the tables that are required for the database to function if they do not already exist.
            /// The tables are for Transactions, Categories, Accounts, and Keywords.
            /// </summary>
            internal static void TablesInit()
            {
                TransactionTableInit();
                CategoryTableInit();
                AccountsTableInit();
                KeywordsTableInit();
            }

            /// <summary>
            /// Creates a Table for the Transactions that should be recorded. Every Transaction row contains:
            /// Id (0), DateTime (1), Amount (2), TransactionType (3), Keywords (4), Category (5), 
            /// Title (6), Note (7), and ImagePath (8).
            /// </summary>
            private static void TransactionTableInit()
            {
                using (var databaseConnection = new Microsoft.Data.Sqlite.SqliteConnection())
                {
                    databaseConnection.ConnectionString = connectionString;
                    databaseConnection.Open();
                    var transactionTable = databaseConnection.CreateCommand();
                    transactionTable.CommandText =
                        @"CREATE TABLE IF NOT EXISTS Transaction_Logs(
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                DateTime TEXT,
                Amount DECIMAL,
                TransactionType INTEGER,
                IsImportant BOOL,
                Keywords TEXT,
                Category INTEGER,
                Title TEXT,
                Note TEXT,
                ImagePath TEXT
                )";
                    transactionTable.ExecuteNonQuery();
                    databaseConnection.Close();
                }
            }

            /// <summary>
            /// Creates a Table for the Categories that should be recorded. Every Category row contains:
            /// Id (0), CategoryType (1), Title (2), IsDefault (3), and Note (4)
            /// </summary>
            private static void CategoryTableInit()
            {
                using (var databaseConnection = new Microsoft.Data.Sqlite.SqliteConnection())
                {
                    databaseConnection.ConnectionString = connectionString;
                    databaseConnection.Open();
                    var categoryTable = databaseConnection.CreateCommand();
                    categoryTable.CommandText =
                        @"CREATE TABLE IF NOT EXISTS Category_Logs(
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                CategoryType INTEGER,
                Title TEXT,
                IsDefault BOOL,
                Note TEXT
                )";
                    categoryTable.ExecuteNonQuery();
                    databaseConnection.Close();
                }
            }

            /// <summary>
            /// Creates a Table for a summary of the transaction Accounts that should be recorded. Every row contains:
            /// Id (0), BeginningDate (1), EndDate (2), ExpensesSum(3), DebtSum(4), OwedSum(5), and EarningsSum (6).
            /// </summary>
            private static void AccountsTableInit()
            {
                using (var databaseConnection = new Microsoft.Data.Sqlite.SqliteConnection())
                {
                    databaseConnection.ConnectionString = connectionString;
                    databaseConnection.Open();
                    var accountsTable = databaseConnection.CreateCommand();
                    accountsTable.CommandText =
                        @"CREATE TABLE IF NOT EXISTS Accounts_Logs(
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                BeginningDate TEXT,
                EndDate TEXT,
                ExpensesSum DECIMAL,
                DebtSum DECIMAL,
                OwedSum DECIMAL,
                EarningSum DECIMAL
                )";
                    accountsTable.ExecuteNonQuery();
                    databaseConnection.Close();
                }
            }

            /// <summary>
            /// Creates a Table for the keywords that are added to the database. Every row contains:
            /// Id (0), and Word (1).
            /// </summary>
            private static void KeywordsTableInit()
            {
                using (var databaseConnection = new Microsoft.Data.Sqlite.SqliteConnection())
                {
                    databaseConnection.ConnectionString = connectionString;
                    databaseConnection.Open();
                    var keywordsTable = databaseConnection.CreateCommand();
                    keywordsTable.CommandText =
                        @"CREATE TABLE IF NOT EXISTS Keywords_Logs(
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Word TEXT
                )";
                    keywordsTable.ExecuteNonQuery();
                    databaseConnection.Close();
                }
            }
        }

        /// <summary>
        /// Initializes the database and the tables required for the application to function.
        /// </summary>
        public static void DatabaseInit ()
        {
            // I didn't want the database creation to be bound to the creation of the tables
            // so I just put this here, so an empty database is created before going for the tables.
            using (var databaseConnection = new Microsoft.Data.Sqlite.SqliteConnection())
            {
                databaseConnection.ConnectionString = connectionString;
                databaseConnection.Open();
                databaseConnection.Close();
            }
            DatabaseTables.TablesInit();
        }

        public static void LoadDatabase ()
        {

        }   
    }
}

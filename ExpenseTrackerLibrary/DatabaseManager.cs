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
    /// <summary>
    /// The class that contains methods for writing into and reading from the database. 
    /// Do Not use this class before Initializing the Database.
    /// </summary>
     public sealed class DatabaseManager
    {
        private static readonly string connectionString = @"Data Source=Expense_Logs.sqlite";
        private static readonly DatabaseManager _instance = new DatabaseManager();
        private static readonly DatabaseWriter _databaseWriter = DatabaseWriter.Instance;
        private static readonly DatabaseReader _databaseReader = DatabaseReader.Instance;

        /// <summary>
        /// Private constructor for the singleton instance.
        /// </summary>
        private DatabaseManager()
        {
            
        }

        /// <summary>
        /// Returns the instance of DatabaseManager class. Singleton.
        /// </summary>
        public static DatabaseManager Instance { get => _instance; }

        /// <summary>
        /// Contains methods for adding, editing, and deleting of the database table elements.
        /// </summary>
        public DatabaseWriter Writer { get => _databaseWriter; }

        /// <summary>
        /// Contains methods for reading from the database and checking the existance of certain elements.
        /// </summary>
        public DatabaseReader Reader { get => _databaseReader; }        
    }
}

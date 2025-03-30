using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTrackerLibrary
{
    /// <summary>
    /// Contains methods for adding, editing, and deleting of the database table elements.
    /// </summary>
    public sealed class DatabaseWriter : BaseDatabaseWriter
    {
        private static readonly DatabaseWriter _databaseWriterInstance = new DatabaseWriter();
        private DatabaseWriter()
        {

        }

        /// <summary>
        /// Returns the instance of DatabaseWriter class.
        /// </summary>
        internal static DatabaseWriter Instance { get => _databaseWriterInstance; }
        // No need for overrides for now as everything is already implemented.
    }
}

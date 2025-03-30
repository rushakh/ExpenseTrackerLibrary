using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTrackerLibrary
{
    /// <summary>
    /// Contains methods for reading from the database and checking the existance of certain elements.
    /// </summary>
    public sealed class DatabaseReader : BaseDatabaseReader
    {
        private static readonly DatabaseReader _databaseReaderInstance = new DatabaseReader();

        private DatabaseReader()
        {

        }

        /// <summary>
        /// Returns the instance of DatabaseReader class.
        /// </summary>
        internal static DatabaseReader Instance { get => _databaseReaderInstance; }
        // No need for overrides for now as everything is already implemented.
    }
}

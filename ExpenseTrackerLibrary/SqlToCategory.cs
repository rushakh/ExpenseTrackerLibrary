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
    /// A class that contains tools for converting the SQLiteReader of the database to objects of type Category.
    /// </summary>
    internal static class SqlToCategory
    {
        /// <summary>
        /// Constructs a Category object using the database element in the SQLite Reader and returns the Category.
        /// If there is only more than one element in the reader, only the first one is converted.
        /// </summary>
        /// <param name="sqliteDataReader"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        internal static Category SQLiteReaderToCategory (SqliteDataReader sqliteDataReader)
        {
            // For the structure of the database and the Rows check the * Database Tables and Columns.txt *
            Category loadedCategory;
            if (sqliteDataReader.HasRows)
            {
                sqliteDataReader.Read();
                int id = sqliteDataReader.GetInt32(0);
                Globals.CategoryTypes categoryType = (CategoryTypes)sqliteDataReader.GetInt32(1);
                string? title = sqliteDataReader.GetString(2);
                bool isDefault = sqliteDataReader.GetBoolean(3);
                string? note = sqliteDataReader.GetString(4);
                loadedCategory = new Category(id, (int)categoryType, title, isDefault, note);
                return loadedCategory;
            }
            else
            {
                throw new ArgumentOutOfRangeException();
            }               
        }

    }
}

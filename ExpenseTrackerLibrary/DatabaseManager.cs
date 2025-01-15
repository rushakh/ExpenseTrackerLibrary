using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTrackerLibrary
{
    internal static class DatabaseManager
    {
        private static class DatabaseWriter
        {
            // Unsure of this design for now.
        }

        private static class DatabaseReader
        {
            internal static Transaction GetAllTransactionsByDate (DateTime from, DateTime until)
            {
                // Not sure if the DatabaseManager class should be like this (nested classes).
                // Think of a suitable structure first, and then write the code for this part.
                return null;
            }
        }

        /// <summary>
        /// Returns an array of type Transaction that contains all the transactions that were set for the specified time period.
        /// Returns null if nothing is found.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="until"></param>
        /// <returns></returns>
        internal static Transaction[] GetAllTransactionsByDate (DateTime from, DateTime until)
        {
            return null;
        }
        /// <summary>
        /// Returns an integar that can be used as an identification number for a new transaction.
        /// </summary>
        /// <returns></returns>
        internal static int GetNewTransactionId ()
        {
            int lastId = 0;
            // Check the last database Entry id and then + 1
            int newId = lastId + 1;
            // OR Directly add the new entry to the database and Then check the id that was determined for it
            // and use that. 2nd way is safer, but has extra steps.
            return newId;
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

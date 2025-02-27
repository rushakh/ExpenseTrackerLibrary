using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTrackerLibrary
{
    /// <summary>
    /// Interface for the Categories that transactions belong to.
    /// </summary>
    internal interface ICategory
    {
        /// <summary>
        /// The identification number used for this category.
        /// </summary>
        int Id { get; }
        /// <summary>
        /// Specifies the type of this category.
        /// </summary>
        Globals.CategoryTypes CategoryType { get; set; }
        /// <summary>
        /// The name of the category.
        /// </summary>
        string Title { get; set; }
        /// <summary>
        /// Specifies whether the Category is one of the default categories or added by the user.
        /// </summary>
        bool isDefaultCategory { get; }
        /// <summary>
        /// Notes and additional information about this category.
        /// </summary>
        string? Note {  get; set; }
        /// <summary>
        /// The transactions that are in this category. Returns null if nothing is found.
        /// </summary>
        Transaction[]? GetTransactions();
    }
}

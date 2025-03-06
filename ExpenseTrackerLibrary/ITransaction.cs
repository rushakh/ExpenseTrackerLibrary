using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTrackerLibrary
{
    /// <summary>
    /// Interface for the 4 types of Transactions; Expense, Earning, Owed, Debts.
    /// </summary>
    internal interface ITransaction
    {
        /// <summary>
        /// The identification Number used for this transaction.
        /// </summary>
        int Id { get; }

        /// <summary>
        /// The date and time on which the transaction took place.
        /// </summary>
        DateTime Date { get; set; }

        /// <summary>
        /// The amount of money involved in the Transaction.
        /// </summary>
        float Amount { get; set; }

        /// <summary>
        /// The type of transaction that took place; either Expense, Earning, Owed, Debt.
        /// </summary>
        Globals.TransactionTypes TransactionType { get; set; }        
        
        /// <summary>
        /// If the user has marked this transaction as important.
        /// </summary>
        bool IsImportant { get; set; }

        /// <summary>
        /// The keywords added by the user that can be used for better searching. Returns string[], or null if nothing was added.
        /// </summary>
        string[]? Keywords { get; set; }

        /// <summary>
        /// The category that the transaction belongs to.
        /// </summary>
        Category Category { get; set; }

        /// <summary>
        /// The optional title chosen for the transaction by the user. Returns null if not set by user, otherwise returns string.
        /// </summary>
        string? Title { get; set; }

        /// <summary>
        /// The note or description added by the user. Returns null if not set by user, otherwise returns a string.
        /// </summary>
        string? Note { get; set; }

        /// <summary>
        /// The location of the Image (a copy andn ot the original file) set by the user for this transaction. Returns null 
        /// if no image was added by the user.
        /// </summary>
        string? ImagePath { get; set; }

        /// <summary>
        /// Specifies whether this transaction contains any keywords added by the user.
        /// </summary>
        bool HasKeywords { get; }

        /// <summary>
        /// If true then the specified transaction contains a note added by the user, 
        /// otherwise, there is no note and Note property is null.
        /// </summary>
        bool HasNote { get; }

        
    }
}

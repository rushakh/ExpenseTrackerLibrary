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
        int Id { get; }
        Globals.TransactionTypes TransactionType { get; set; }
        DateTime Date { get; set; }
        bool HasKeywords { get; }
        string[] Keywords { get; set; }
        string Title { get; set; }
        Category Category { get; set; }
        string Note {  get; set; }
        bool HasImage { get; }
        string ImagePath { get; set; }
        // Image
    }
}

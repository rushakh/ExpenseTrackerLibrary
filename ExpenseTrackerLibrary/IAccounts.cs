using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTrackerLibrary
{
    /// <summary>
    /// Interface for the financial Accounts in a specified time period. 
    /// </summary>
    internal interface IAccounts
    {
        /// <summary>
        /// The date from which the accounts start.
        /// </summary>
        DateTime Beginning { get; }

        /// <summary>
        /// The date up to which the accounts extend.
        /// </summary>
        DateTime End { get; }

        /// <summary>
        /// All of the transactions that were recorded for this period of time. Returns an array of type Transaction.
        /// Returns null if no transaction is found.
        /// </summary>
        Transaction[]? Transactions { get; }

        /// <summary>
        /// The total amount of recorded Expenses for the specified period of time. 
        /// </summary>
        int ExpenseSum { get; }

        /// <summary>
        /// The total amount of recorded Debts for the specified period of time.
        /// </summary>
        int DebtSum { get; }

        /// <summary>
        /// The total amount of recorded Oweds (by others) for the specified period of time.
        /// </summary>
        int OwedSum { get; }

        /// <summary>
        /// The total amount of recorded Earnings for the specified period of time.
        /// </summary>
        int EarningSum { get; }

        /// <summary>
        /// All of the recorded Transactions that were marked as Expense for the specified period of time. Returns
        /// an array of type Transaction. Returns null if no such transaction is found.
        /// </summary>
        /// <returns></returns>
        Transaction[]? Expenses();

        /// <summary>
        /// All of the recorded Transactions that were marked as Debt for the specified period of time. Returns
        /// an array of type Transaction. Returns null if no such transaction is found.
        /// </summary>
        /// <returns></returns>
        Transaction[]? Debts();

        /// <summary>
        /// All of the recorded Transactions that were marked as Owed for the specified period of time. Returns
        /// an array of type Transaction. Returns null if no such transaction is found.
        /// </summary>
        /// <returns></returns>
        Transaction[]? Oweds();

        /// <summary>
        /// All of the recorded Transactions that were marked as Earning for the specified period of time. Returns
        /// an array of type Transaction. Returns null if no such transaction is found.
        /// </summary>
        /// <returns></returns>
        Transaction[]? Earnings();
    }
}

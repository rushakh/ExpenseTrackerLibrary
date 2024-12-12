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
        string Title { get; set; }
        bool isDefaultCategory { get; }
        bool hasSubcategory { get; }
        int ExpensesSum { get; }
        int EarningsSum { get; }
        int OwedSum { get; }
        int DebtSum { get; }
        string Note {  get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTrackerLibrary
{
    /// <summary>
    /// The main groups that the transactions can be divided into for various purposes.
    /// </summary>
    public class Category: ICategory
    {
        private int _id;
        private string _title;
        private bool _isDefaultCategory;
        private bool _hasSubcategory;
        private int _expensesSum;
        private int _earningsSum;
        private int _owedSum;
        private int _debtSum;
        private string? _note;
        // The sums should later be changed so that they are calculated in the accessor instead of being private properties
        // since this makes things complicated a bit and is unnecessary at this stage.
        // LATER should also add IMAGE AND COLOR Attributes Or maybe that should be done somewhere else.

        public int Id { get => _id; }

        public string Title { get => _title; set => _title = value; }

        public bool isDefaultCategory { get => _isDefaultCategory; }

        public bool hasSubcategory {get => _hasSubcategory;}

        public int ExpensesSum => throw new NotImplementedException();

        public int EarningsSum => throw new NotImplementedException();

        public int OwedSum => throw new NotImplementedException();

        public int DebtSum => throw new NotImplementedException();

        public string? Note { get => _note; set => _note = value; }

        public Transaction[]? Transactions { get => throw new NotImplementedException(); }

        /// <summary>
        /// Constructor for loading already existing Categories.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="title"></param>
        /// <param name="isDefault"></param>
        /// <param name="hasSubs"></param>
        /// <param name="expenseSum"></param>
        /// <param name="earningSum"></param>
        /// <param name="owedSum"></param>
        /// <param name="debtSum"></param>
        /// <param name="note"></param>
        public Category (int id, string title, bool isDefault, bool hasSubs, int expenseSum, int earningSum, int owedSum, int debtSum, string note)
        {
            _id = id;
            _title = title;
            _isDefaultCategory = isDefault;
            _hasSubcategory = hasSubs;
            _expensesSum = expenseSum;
            _earningsSum = earningSum;
            _owedSum = owedSum;
            _debtSum = debtSum;
            _note = note;
        }

        /// <summary>
        /// Constructor that allows for constructing new Category objects.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="isDefault"></param>
        /// <param name="note"></param>
        public Category (string title, bool isDefault, string note)
        {
            _id = DatabaseManager.GetNewCategoryId();
            _title = title;
            _note = note;
            _isDefaultCategory = isDefault;
        }       
    }
}

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
        private Globals.CategoryTypes _categoryType;
        private string _title;
        private bool _isDefaultCategory;
        private string? _note;

        // LATER should also add IMAGE AND COLOR Attributes Or maybe that should be done somewhere else.

        public int Id { get => _id; }

        public Globals.CategoryTypes CategoryType { get => _categoryType; set => _categoryType = value;}

        public string Title { get => _title; set => _title = value; }

        public bool isDefaultCategory { get => _isDefaultCategory; }

        public string? Note { get => _note; set => _note = value; }

        /// <summary>
        /// Constructor that allows for constructing new Category objects.
        /// </summary>
        /// <param name="categoryType"></param>
        /// <param name="title"></param>
        /// <param name="isDefault"></param>
        /// <param name="note"></param>
        public Category(Globals.CategoryTypes categoryType, string title, bool isDefault, string? note)
        {
            _categoryType = categoryType;
            _title = title;
            _note = note;
            _isDefaultCategory = isDefault;
            _id = DatabaseManager.DatabaseWriter.AddCategory(_categoryType, _title, _isDefaultCategory, _note);
        }

        /// <summary>
        /// Constructor for loading already existing Categories.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="categoryType"></param>
        /// <param name="title"></param>
        /// <param name="isDefault"></param>
        /// <param name="note"></param>
        public Category (int id, int categoryType, string title, bool isDefault, string? note)
        {
            _id = id;
            _categoryType = (Globals.CategoryTypes)categoryType;
            _title = title;
            _isDefaultCategory = isDefault;
            _note = note;
        }       

        public Transaction[]? GetTransactions ()
        {
            var categoryTransactions = DatabaseManager.DatabaseReader.GetTransactions(this);
            return categoryTransactions;
        }
    }
}

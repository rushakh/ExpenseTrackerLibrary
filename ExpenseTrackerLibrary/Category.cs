﻿using System;
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

        /// <inheritdoc/>
        public int Id { get => _id; }
        /// <inheritdoc/>
        public Globals.CategoryTypes CategoryType { get => _categoryType; set => _categoryType = value;}
        /// <inheritdoc/>
        public string Title 
        { 
            get 
            { 
                return _title;
            }
            set 
            {
                if (value is not null && value != string.Empty)
                {
                    if (value == _title) { _title = value; }
                    else if (FormatAndFilter.IsCategoryAllowed(value)) { _title = value; }
                    else { throw new ArgumentException("The input string is not permitted. Either a Category with that name already exists or the string contains characters that are not allowed."); }
                }
                else
                {
                    throw new ArgumentException("Title cannot be null, empty string, or a duplicate.");
                }
            }
        }
        /// <inheritdoc/>
        public bool isDefaultCategory { get => _isDefaultCategory; }
        /// <inheritdoc/>
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
            if (!FormatAndFilter.IsCategoryAllowed(title))
            {
                throw new ArgumentException("The Category either exists or the title contains invalid elements.");
            }
            _title = title;
            _note = note;
            _isDefaultCategory = isDefault;
            _id = Globals.Database.Writer.AddCategory(_categoryType, _title, _isDefaultCategory, _note);
        }

        /// <summary>
        /// Constructor for loading already existing Categories.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="categoryType"></param>
        /// <param name="title"></param>
        /// <param name="isDefault"></param>
        /// <param name="note"></param>
        internal Category (int id, int categoryType, string title, bool isDefault, string? note)
        {
            _id = id;
            _categoryType = (Globals.CategoryTypes)categoryType;
            _title = title;
            _isDefaultCategory = isDefault;
            _note = note;
        }

        /// <inheritdoc/>
        public Transaction[]? GetTransactions ()
        {
            var categoryTransactions = Globals.Database.Reader.GetTransactions(this);
            return categoryTransactions;
        }

        /// <inheritdoc/>
        public bool Update ()
        {
            int affectedRows = Globals.Database.Writer.UpdateCategory(this);
            if (affectedRows == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <inheritdoc/>
        public bool Remove ()
        {
            int affectedRows = Globals.Database.Writer.DeleteCategory(Id);
            if (affectedRows == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

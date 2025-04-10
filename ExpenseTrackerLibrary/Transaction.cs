using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTrackerLibrary
{
    /// <summary>
    /// The class for construction Transaction objects, that are used for populating the database using the user's transactions.
    /// </summary>
    public class Transaction : ITransaction
    {
        private int _id;
        private Globals.TransactionTypes _transactionType;
        private DateTime _dateTime;
        private decimal _amount;
        private bool _isImportant;
        private bool _hasKeywords;
        string[]? _keywords;
        private string? _title;
        private Category _category;
        private bool _hasNote;
        private string? _note;
        private string? _imagePath;

        /// <inheritdoc/>
        public int Id { get => _id; }
        /// <inheritdoc/>
        public DateTime Date { get => _dateTime; set => _dateTime = value; }
        /// <inheritdoc/>
        public decimal Amount { get => _amount; set => _amount = value; }
        /// <inheritdoc/>
        public Globals.TransactionTypes TransactionType { get => _transactionType; set => _transactionType = value; }
        /// <inheritdoc/>
        public bool IsImportant { get => _isImportant; set => _isImportant = value; }
        /// <inheritdoc/>
        public string[]? Keywords 
        {
            get 
            { 
                return _keywords;
            }
            set 
            {
                if (value is null)
                {
                    _keywords = null;
                    _hasKeywords = false;
                }
                else
                {
                    if (value.Length > 0 && FormatAndFilter.AreKeywordsAllowed(value))
                    {
                        string[] readyKeywords = FormatAndFilter.AddHashToKeywords(value);
                        _keywords = readyKeywords;
                        _hasKeywords = true;
                    }
                    else
                    {
                        _hasKeywords = false;
                        throw new ArgumentException("Either The string array is empty or the elements contain characters that are not allowed.");
                    }
                }                 
            } 
        }
        /// <inheritdoc/>
        public Category Category { get => _category; set => _category = value; }
        /// <inheritdoc/>
        public string? Title
        {
            get 
            { 
                return _title;
            }
            set 
            {
                if (value is null)
                {
                    _title = null;
                }
                else
                {
                    if (FormatAndFilter.IsTransactionTitleAllowed(value))
                    {
                        _title = value;
                    }
                    else
                    {
                        throw new ArgumentException("Input string contains characters that are not allowed.");
                    }
                }
            }
        }
        /// <inheritdoc/>
        public string? Note 
        {
            get 
            { 
                return _note;
            }
            set 
            {
                _note = value;
                if (_note is not null && _note != string.Empty)
                {
                    _hasNote = true;
                }
                else
                {
                    _hasNote = false;
                }
            } 
        }
        /// <inheritdoc/>
        public string? ImagePath { get => _imagePath; set => _imagePath = value; }
        /// <inheritdoc/>
        public bool HasKeywords { get => _hasKeywords; }
        /// <inheritdoc/>
        public bool HasNote { get => _hasNote; }

        /// <summary>
        /// Constructor that should be used to construct a new object of type Transaction.
        /// </summary>
        /// <param name="dateAndTime"></param>
        /// <param name="amount"></param>
        /// <param name="type"></param>
        /// <param name="isImportant"></param>
        /// <param name="keywords"></param>
        /// <param name="category"></param>
        /// <param name="title"></param>
        /// <param name="note"></param>
        /// <param name="imagePath"></param>
        public Transaction (DateTime dateAndTime, decimal amount, Globals.TransactionTypes type, bool isImportant, string[]? keywords, Category category, string? title, string? note, string? imagePath)
        {
            _transactionType = type;
            _dateTime = dateAndTime;
            _amount = amount;
            _isImportant = isImportant;
            // Keywords should be checked
            if (keywords != null && keywords.Length>0)
            {
                if (FormatAndFilter.AreKeywordsAllowed(keywords))
                {
                    string[] readyKeywords = FormatAndFilter.AddHashToKeywords(keywords);
                    _keywords = readyKeywords;
                    if (_keywords is not null && _keywords.Length > 0)
                    {
                        _hasKeywords = true;
                    }
                    else
                    {
                        _hasKeywords = false;
                    }
                }
                else
                {
                    throw new ArgumentException("keywords[] contains at least one invalid elements");
                }
            }
            else
            {
                _keywords = null;
                _hasKeywords = false;
            }
            // Title should be checked
            if (title != null && title != string.Empty)
            {
                if (FormatAndFilter.IsTransactionTitleAllowed(title))
                {
                    _title = title;
                }
                else
                {
                    throw new ArgumentException("title string contains at least one invalid element.");
                }
            }
            else
            {
                _title = title;
            }
            _category = category;
            _note = note;
            if (_note is not null && _note != string.Empty)
            {
                _hasNote = true;
            }
            else
            {
                _hasNote = false;
            }
            _imagePath = imagePath;
            // Should add the Transaction's data to the database, and get the Id now.
            _id = Globals.Database.Writer.AddTransaction(_dateTime, _amount, _transactionType, _isImportant, _keywords, _category, _title, _note, _imagePath);
        }

        /// <summary>
        /// Constructor that should be used for loading a Transaction from the database.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dateAndTime"></param>
        /// <param name="amount"></param>
        /// <param name="type"></param>
        /// <param name="isImportant"></param>
        /// <param name="keywords"></param>
        /// <param name="category"></param>
        /// <param name="title"></param>
        /// <param name="note"></param>
        /// <param name="imagePath"></param>
        internal Transaction (int id, DateTime dateAndTime, decimal amount, Globals.TransactionTypes type,  bool isImportant, string[]? keywords, Category category, string? title,  string? note, string? imagePath)
        {
            _id = id;
            _transactionType = type;
            _dateTime = dateAndTime;
            _amount = amount;
            _isImportant = isImportant;
            _keywords = keywords;
            if (_keywords is not null && _keywords.Length > 0)
            {
                _hasKeywords = true;
            }
            else
            {
                _hasKeywords = false;
            }
            _title = title;
            _category = category;
            _note = note;
            if (_note is not null && _note != string.Empty)
            {
                _hasNote = true;
            }
            else
            {
                _hasNote = false;
            }
            _imagePath = imagePath;
        }

        /// <inheritdoc/>
        public bool Update ()
        {
            int affectedRows = Globals.Database.Writer.UpdateTransaction(this);
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
            int affectedRows = Globals.Database.Writer.DeleteTransaction(Id);
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

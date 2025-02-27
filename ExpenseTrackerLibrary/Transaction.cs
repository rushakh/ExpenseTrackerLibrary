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
        private float _amount;
        private bool _isImportant;
        private bool _hasKeywords;
        string[]? _keywords;
        private string? _title;
        private Category _category;
        private bool _hasNote;
        private string? _note;
        private string? _imagePath;

        public int Id { get => _id; }

        public DateTime Date { get => _dateTime; set => _dateTime = value; }

        public float Amount { get => _amount; set => _amount = value; }

        public Globals.TransactionTypes TransactionType { get => _transactionType; set => _transactionType = value; }

        public bool IsImportant { get => _isImportant; set => _isImportant = value; }

        public string[]? Keywords { get => _keywords; set => _keywords = value; }

        public Category Category { get => _category; set => _category = value; }

        public string? Title { get => _title; set => _title = value; }

        public string? Note { get => _note; set => _note = value; }

        public string? ImagePath { get => _imagePath; set => _imagePath = value; }

        public bool HasKeywords { get => _hasKeywords; }     

        public bool HasNote
        {
            get
            {
                if (_note is null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        /// <summary>
        /// Constructor that should be used to construct a new object of type Transaction.
        /// </summary>
        /// <param name="date"></param>
        /// <param name="amount"></param>
        /// <param name="type"></param>
        /// <param name="isImportant"></param>
        /// <param name="keywords"></param>
        /// <param name="category"></param>
        /// <param name="title"></param>
        /// <param name="note"></param>
        /// <param name="imagePath"></param>
        public Transaction (DateTime date, float amount, Globals.TransactionTypes type, bool isImportant, string[]? keywords, Category category, string? title, string? note, string? imagePath)
        {
            // Databasemanger class and the method that is used has not been written yet***.
            _id = DatabaseManager.GetNewTransactionId();
            _transactionType = type;
            _dateTime = date;
            _amount = amount;
            _isImportant = isImportant;
            _keywords = keywords;
            if (_keywords is null)
            {
                _hasKeywords = false;
            }
            else
            {
                _hasKeywords = true;
            }
            _title = title;
            _category = category;
            _note = note;
            if (_note is null)
            {
                _hasNote = false;
            }
            else
            {
                _hasNote = true;
            }
            _imagePath = imagePath;
        }

        /// <summary>
        /// Constructor that should be used for loading a Transaction from the database.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="date"></param>
        /// <param name="amount"></param>
        /// <param name="type"></param>
        /// <param name="isImportant"></param>
        /// <param name="keywords"></param>
        /// <param name="category"></param>
        /// <param name="title"></param>
        /// <param name="note"></param>
        /// <param name="imagePath"></param>
        public Transaction (int id, DateTime date, float amount, Globals.TransactionTypes type,  bool isImportant, string[]? keywords, Category category, string? title,  string? note, string? imagePath)
        {
            _id = id;
            _transactionType = type;
            _dateTime = date;
            _amount = amount;
            _isImportant = isImportant;
            _keywords = keywords;
            if (_keywords is null)
            {
                _hasKeywords = false;
            }
            else
            {
                _hasKeywords = true;
            }
            _title = title;
            _category = category;
            _note = note;
            if (_note is null)
            {
                _hasNote = false;
            }
            else
            {
                _hasNote = true;
            }
            _imagePath = imagePath;
        }
    }
}

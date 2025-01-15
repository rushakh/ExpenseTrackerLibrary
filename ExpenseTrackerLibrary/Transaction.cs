using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

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
        private int _amount;
        private bool _isImportant;
        private bool _hasKeywords;
        string[]? _keywords;
        private string? _title;
        private Category _category;
        private bool _hasNote;
        private string? _note;
        private bool _hasImage;
        private string? _imagePath;
        private Image?  _image;

        public int Id { get => _id; }
       
        public Globals.TransactionTypes TransactionType { get => _transactionType; set => _transactionType = value; }
        
        public DateTime Date { get => _dateTime; set => _dateTime = value; }

        public int Amount { get => _amount; set => _amount = value; }

        public bool IsImportant { get => _isImportant; set => _isImportant = value; }

        public bool HasKeywords { get => _hasKeywords; }

        public string[]? Keywords { get => _keywords; set => _keywords = value; }

        public string? Title { get => _title; set => _title = value; }

        public Category Category { get => _category; set => _category = value; }

        public string? Note { get => _note; set => _note = value; }

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

        public bool HasImage {  get => _hasImage; }

        public string? ImagePath { get => _imagePath; set => _imagePath = value; }

        public Image Image { get => _image; set => _image = value; }

        /// <summary>
        /// Constructor that should be used to construct a new object of type Transaction.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="date"></param>
        /// <param name="amount"></param>
        /// <param name="isImportant"></param>
        /// <param name="keywords"></param>
        /// <param name="title"></param>
        /// <param name="category"></param>
        /// <param name="note"></param>
        /// <param name="imagePath"></param>
        /// <param name="image"></param>
        public Transaction (Globals.TransactionTypes type, DateTime date, int amount, bool isImportant, string[]? keywords, string? title, Category category, string? note, string? imagePath, Image? image)
        {
            // Databasemanger class and the method that is used has not been written yet***.
            _id = DatabaseManager.GetNewTransactionId();
            _transactionType = type;
            _dateTime = date;
            _amount = amount;
            _isImportant = isImportant;
            _keywords = keywords;
            // SHOULD CHANGE *** should set Image to be loaded using the path, instead of loading it from the beginning.
            _image = image;
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
            if (_imagePath is null)
            {
                _hasImage = false;
            }
            else
            {
                _hasImage = true;
            }
        }

        /// <summary>
        /// Constructor that should be used for loading a Transaction from the database.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <param name="date"></param>
        /// <param name="amount"></param>
        /// <param name="isImportant"></param>
        /// <param name="keywords"></param>
        /// <param name="title"></param>
        /// <param name="category"></param>
        /// <param name="note"></param>
        /// <param name="imagePath"></param>
        /// <param name="image"></param>
        public Transaction (int id, Globals.TransactionTypes type, DateTime date, int amount, bool isImportant, string[]? keywords, string? title, Category category, string? note, string? imagePath, Image? image)
        {
            _id = id;
            _transactionType = type;
            _dateTime = date;
            _amount = amount;
            _isImportant = isImportant;
            _keywords = keywords;
            // SHOULD CHANGE *** should set Image to be loaded using the path, instead of loading it from the beginning.
            _image = image;
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
            if (_imagePath is null)
            {
                _hasImage = false;
            }
            else
            {
                _hasImage = true;
            }
        }
    }
}

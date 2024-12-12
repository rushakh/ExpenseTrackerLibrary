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
    internal class Transaction : ITransaction
    {
        private int _id;
        private Globals.TransactionTypes _transactionType;
        private DateTime _dateTime;
        private bool _hasKeywords;
        string[]? _keywords;
        private string? _title;
        private Category _category;
        private bool _hasNote;
        private string? _note;
        private bool _hasImage;
        private string? _imagePath;
        // add Image after System.Drawing issue has been resolved.

        /// <summary>
        /// The identification Number used for this transaction.
        /// </summary>
        public int Id { get => _id; }

        /// <summary>
        /// The type of transaction that took place; either Expense, Earning, Owed, Debt. 
        /// </summary>
        public Globals.TransactionTypes TransactionType { get => _transactionType; set => _transactionType = value; }

        /// <summary>
        /// The date and time on which the transaction took place.
        /// </summary>
        public DateTime Date { get => _dateTime; set => _dateTime = value; }

        /// <summary>
        /// Specifies whether this transaction contains any keywords added by the user.
        /// </summary>
        public bool HasKeywords { get => _hasKeywords; }

        /// <summary>
        /// The keywords added by the user that can be used for better searching. Returns string[], or null if nothing was added.
        /// </summary>
        public string[]? Keywords { get => _keywords; set => _keywords = value; }

        /// <summary>
        /// The optional title chosen for the transaction by the user. Returns null if not set by user, otherwise returns string.
        /// </summary>
        public string? Title { get => _title; set => _title = value; }

        /// <summary>
        /// The category that the transaction belongs to.
        /// </summary>
        public Category Category { get => Category; set => Category = value; }

        /// <summary>
        /// The note or description added by the user. Returns null if not set by user, otherwise returns a string.
        /// </summary>
        public string? Note { get => _note; set => _note = value; }

        /// <summary>
        /// Specifies whether this transaction contains an image added by the user. If false, ImagePath and Image will return null.
        /// </summary>
        public bool HasImage {  get => _hasImage; }

        /// <summary>
        /// The location of the Image (a copy andn ot the original file) set by the user for this transaction. Returns null 
        /// if no image was added by the user.
        /// </summary>
        public string? ImagePath { get => _imagePath; set => _imagePath = value; }

        //public Image Image { get => _image; set => _image = value; }

        /// <summary>
        /// Constructor that should be used for loading a Transaction from the database.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <param name="date"></param>
        /// <param name="keywords"></param>
        /// <param name="title"></param>
        /// <param name="category"></param>
        /// <param name="note"></param>
        /// <param name="imagePath"></param>
        public Transaction (int id, Globals.TransactionTypes type, DateTime date, string[]? keywords, string? title, Category category, string? note, string? imagePath)
        {
            
        }
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using ExpenseTrackerLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace ExpenseTrackerLibrary.Tests
{
    [TestClass()]
    public class TransactionTests
    {
        [TestMethod()]
        public void TransactionTest()
        {
            DatabaseManager dbManager = DatabaseManager.Instance;
            // First We will clear the database of all other transactions.
            dbManager.Writer.DeleteAllTransactions();
            // the transaction Id will be there after constructing the object, as it is received
            // from the database.
            int testId;
            // The values --> date in dd/mm/yyyy format
            DateTime testDateTime = DateTime.Parse("28/03/2025 12:05:20");
            Category testCategory = new Category(Globals.CategoryTypes.MainCategory, "For Tests", false, "Made only in order to contain test inputs.");
            decimal testAmount = 126.45m;
            string testTitle = "Unit Test";
            string testNote = "This is a test";
            string? testImagePath = null; // No need for this at this point
            bool testIsImportant = true;
            Globals.TransactionTypes testTransactionType = Globals.TransactionTypes.Expense;
            string[] testKeywords = {"Test", "Yay" };
            // Now to construct the object
            Transaction testTransaction = 
                new Transaction(testDateTime, testAmount, testTransactionType, testIsImportant,
                testKeywords, testCategory, testTitle, testNote, testImagePath);
            Assert.IsNotNull(testTransaction);
            testId = testTransaction.Id;
            // Now to check
            Assert.AreEqual<Globals.TransactionTypes>(testTransactionType, testTransaction.TransactionType);
            Assert.AreEqual<DateTime>(testDateTime, testTransaction.Date);
            Assert.AreEqual<Category>(testCategory, testTransaction.Category);
            Assert.AreEqual<decimal>(testAmount, testTransaction.Amount);
            Assert.AreEqual<bool>(testIsImportant, testTransaction.IsImportant);
            Assert.AreEqual<string>(testTitle, testTransaction.Title);
            Assert.AreEqual<string[]>(testKeywords, testTransaction.Keywords);
            Assert.AreEqual<string>(testNote, testTransaction.Note);
            Assert.IsTrue(testTransaction.HasNote);
            Assert.IsTrue(testTransaction.HasKeywords);
            // We can now change some of the values and test again.
            // ***
            // Now that we're done, we'll delete the added Transaction.
            dbManager.Writer.DeleteTransaction(testId);
        }
    }
}
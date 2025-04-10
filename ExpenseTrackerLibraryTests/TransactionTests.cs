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
            DatabaseInitialization.DatabaseInit();
            // First We will clear the database of all other transaction, categories, etc.s.
            dbManager.Writer.DeleteAllTransactions();
            dbManager.Writer.DeleteAllCategories();
            dbManager.Writer.DeleteAllKeywords();
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
            string[] testKeywords = {"#Test", "#Yay" };
            // Now to construct the object
            Transaction testTransaction = 
                new Transaction(testDateTime, testAmount, testTransactionType, testIsImportant,
                testKeywords, testCategory, testTitle, testNote, testImagePath);
            Assert.IsNotNull(testTransaction);
            testId = testTransaction.Id;
            // Now to check
            Assert.AreEqual<Globals.TransactionTypes>(testTransactionType, testTransaction.TransactionType);
            Assert.AreEqual<DateOnly>(DateOnly.FromDateTime(testDateTime), DateOnly.FromDateTime(testTransaction.Date));
            Assert.AreEqual<Category>(testCategory, testTransaction.Category);
            Assert.AreEqual<decimal>(testAmount, testTransaction.Amount);
            Assert.AreEqual<bool>(testIsImportant, testTransaction.IsImportant);
            Assert.AreEqual<string>(testTitle, testTransaction.Title);
            Assert.AreEqual<int>(2, testTransaction.Keywords.Length);
            Assert.AreEqual<string>(testKeywords[0], testTransaction.Keywords[0]);
            Assert.AreEqual<string>(testKeywords[1], testTransaction.Keywords[1]);
            Assert.AreEqual<string>(testNote, testTransaction.Note);
            Assert.IsTrue(testTransaction.HasNote);
            Assert.IsTrue(testTransaction.HasKeywords);
            // We can now change some of the values and test again.
            // ***

            // Now that we're done, we'll delete the added Transaction.
            dbManager.Writer.DeleteTransaction(testId);
        }

        [TestMethod()]
        public void UpdateTest()
        {
            DatabaseManager dbManager = DatabaseManager.Instance;
            DatabaseInitialization.DatabaseInit();
            dbManager.Writer.DeleteAllCategories();
            dbManager.Writer.DeleteAllTransactions();

            Category testCategory = new Category(Globals.CategoryTypes.MainCategory, "Unit Tests", false, null);
            DateTime testDateTime1 = DateTime.MinValue;
            decimal testAmount1 = 10.00m;   
            string testTitle1 = "Test 1 of 2";
            bool testIsImportant1 = true;
            DateTime testDateTime2 = DateTime.MaxValue;
            decimal testAmount2 = 15.45m;
            string testTitle2 = "Test 2 of 2";
            bool testIsImportant2 = false;
            Transaction testTransaction1 =
                new Transaction(testDateTime1, testAmount1, Globals.TransactionTypes.Expense, testIsImportant1, null, testCategory, testTitle1, null, null);

            Assert.IsNotNull(testTransaction1);
            // We will now make changes and get that Transaction from the database again
            testTransaction1.Date = testDateTime2;
            testTransaction1.Amount = testAmount2;
            testTransaction1.IsImportant = testIsImportant2;
            testTransaction1.Title = testTitle2;
            testTransaction1.Update();
            Transaction updatedTestTransaction1 = dbManager.Reader.GetTransaction(testTransaction1.Id);
            // We can change other things as well but no need for that now.
            Assert.AreEqual<string>(testTitle2, updatedTestTransaction1.Title);
            Assert.AreEqual<decimal>(testAmount2, updatedTestTransaction1.Amount);
            Assert.AreEqual<DateOnly>(DateOnly.FromDateTime(testDateTime2), DateOnly.FromDateTime(updatedTestTransaction1.Date));
            Assert.AreEqual<bool>(testIsImportant2, updatedTestTransaction1.IsImportant);
            // We can now delete everything
            dbManager.Writer.DeleteAllTransactions();
            dbManager.Writer.DeleteAllCategories();
        }

        [TestMethod()]
        public void RemoveTest()
        {
            DatabaseManager dbManager = DatabaseManager.Instance;
            DatabaseInitialization.DatabaseInit();
            dbManager.Writer.DeleteAllTransactions();

            Category testCategory = new Category(Globals.CategoryTypes.MainCategory, "Unit Tests", false, null);            
            // There should be no transaction in the database.
            Transaction[]? foundTransactions = dbManager.Reader.GetAllTransactions();
            Assert.IsNull(foundTransactions);
            // We will add two transactions, test, then remove 1 of them and test again.
            Transaction testTransaction1 = 
                new Transaction(DateTime.MinValue, 10m, Globals.TransactionTypes.Expense, true, null, testCategory, "test1", null, null);
            Transaction testTransaction2 =
                new Transaction(DateTime.MaxValue, 20m, Globals.TransactionTypes.Expense, true, null, testCategory, "test2", null, null);
            Transaction[]? foundTransactions2 = dbManager.Reader.GetAllTransactions();
            Assert.IsNotNull(foundTransactions2);
            Assert.IsTrue(foundTransactions2.Length == 2);
            testTransaction1.Remove();
            foundTransactions2 = dbManager.Reader.GetAllTransactions();
            Assert.IsNotNull(foundTransactions2);
            Assert.IsTrue(foundTransactions2.Length == 1);
            Assert.AreEqual<decimal>(testTransaction2.Amount, foundTransactions2[0].Amount);
            // we can now delete everything.
            dbManager.Writer.DeleteAllTransactions();
            dbManager.Writer.DeleteAllCategories();
        }
    }
}
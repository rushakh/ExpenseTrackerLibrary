using Microsoft.VisualStudio.TestTools.UnitTesting;
using ExpenseTrackerLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTrackerLibrary.Tests
{
    [TestClass()]
    public class CategoryTests
    {
        [TestMethod()]
        public void CategoryTest()
        {
            // When the database is initialized, default Category objects are constructed
            // and then added to the database.
            // So if no exxception is encountered after this one line, then it's working.
            // But That might not be the case in all situations (categories deleted manually, etc.)
            // So just in case, we can test it further.
            DatabaseManager dbManager = DatabaseManager.Instance;
            // Check to see if the default categories that should have been created exist.
            
            
            // Then we construct a category object with test values and test again.
            // The Id is received from the database after the category has
            // been added (automatically after construction)
            int testCategoryId;
            string testTitle = "testCategory";
            Globals.CategoryTypes testCategoryType = Globals.CategoryTypes.MainCategory;
            bool testIsDefaultCategory = false;
            string? note = "This is a category that was created for testing.";
            Category testCategory = new Category(testCategoryType, testTitle, testIsDefaultCategory, note);
            
            Assert.IsNotNull(testCategory);
            testCategoryId = testCategory.Id;
            Assert.IsTrue(dbManager.Reader.CategoryExists(testTitle));
            Assert.AreEqual(testCategoryType, testCategory.CategoryType);
            Assert.AreEqual(testTitle, testCategory.Title);
            Assert.AreEqual(testIsDefaultCategory, testCategory.isDefaultCategory);
            Assert.AreEqual(note, testCategory.Note);
            // We can now change values and test again.
            // ***
            // Now that we are done, we can delete the Category.
            dbManager.Writer.DeleteCategory(testCategoryId);
        }

        [TestMethod()]
        public void GetTransactionsTest()
        {
            DatabaseManager dbManager = DatabaseManager.Instance;
            Category testCategory = new Category(Globals.CategoryTypes.MainCategory, "Test Category 2", false, null);
            Assert.IsNotNull(testCategory);
            // As there are no transactions in this category, if we call the method
            // it should return null.
            Transaction[]? testCategoryTrasactions = testCategory.GetTransactions();
            Assert.IsNull(testCategoryTrasactions);
            // We can then add Transactions and test again.
            Transaction testTransaction = new Transaction(DateTime.Now, 12.50m, Globals.TransactionTypes.Expense, false, null, testCategory, null, null, null);
            Assert.IsNotNull(testTransaction);
            // If we call the method now, it should not be null and should have the length of 1.
            testCategoryTrasactions = testCategory.GetTransactions();
            int testCategoryTransactionsLength = 1;
            Assert.IsNotNull(testCategoryTrasactions);
            Assert.AreEqual(testCategoryTransactionsLength, testCategoryTrasactions.Length);
            Assert.AreEqual(testTransaction.Id, testCategoryTrasactions[0].Id);
            // Now that we're done, we can delete them all.
            dbManager.Writer.DeleteAllCategories();
            dbManager.Writer.DeleteAllTransactions();
        }
    }
}
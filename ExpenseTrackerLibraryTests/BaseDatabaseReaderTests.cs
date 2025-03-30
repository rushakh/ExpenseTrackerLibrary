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
    public class BaseDatabaseReaderTests
    {
        [TestMethod()]
        public void HasCategoriesTest()
        {
            DatabaseManager dbManager = DatabaseManager.Instance;
            // After the database is created, it is filled with default categories
            // therefor it should return true at first but since at the end of the test,
            // we delete all Categories, it will only work the first time, unless we call
            // for the database to be initialized before that.
            DatabaseInitialization.DatabaseInit();
            Assert.IsTrue(dbManager.Reader.HasCategories());
            // We'll then delete all the categories and check again.
            dbManager.Writer.DeleteAllCategories();
            Assert.IsFalse(dbManager.Reader.HasCategories());
        }

        [TestMethod()]
        public void GetAllTransactionsTest()
        {
            DatabaseManager dbManager = DatabaseManager.Instance;
            // It should return Null at first as there are no transaction in it
            Transaction[]? testAllTransactions = dbManager.Reader.GetAllTransactions();
            Assert.IsNull(testAllTransactions);
            Category testCategory1 = new Category(Globals.CategoryTypes.MainCategory, "Test Category 1", false, null);
            Category testCategory2 = new Category(Globals.CategoryTypes.MainCategory, "Test Category 2", false, null);

            Transaction testTransaction1 = new Transaction(DateTime.Now, 10.20m, Globals.TransactionTypes.Expense, true, null, testCategory1, "Test Transaction 1", null, null);
            Transaction testTransaction2 = new Transaction(DateTime.Now, 15.60m, Globals.TransactionTypes.Expense, true, null, testCategory2, "Test Transaction 2", null, null);
            // The database should no longer be empty, so we can test it again.
            testAllTransactions = dbManager.Reader.GetAllTransactions();
            Assert.IsNotNull (testAllTransactions);
            // It should have the length of 2.
            Assert.AreEqual (2, testAllTransactions.Length);
            // We can now delete the transactions, and test it once more.
            dbManager.Writer.DeleteAllTransactions();
            testAllTransactions = dbManager.Reader.GetAllTransactions();
            Assert.IsNull (dbManager.Reader.GetAllTransactions());
            dbManager.Writer.DeleteAllCategories();
        }

        [TestMethod()]
        public void GetAllCategoriesTest()
        {
            DatabaseManager dbManager = DatabaseManager.Instance;
            DatabaseInitialization.DatabaseInit();
            // Default categories should exist after initializations
            Category[]? testAllCategories = dbManager.Reader.GetAllCategories();
            Assert.IsNotNull(testAllCategories);
            // We can now delete them.
            dbManager.Writer.DeleteAllCategories ();
            testAllCategories = dbManager.Reader.GetAllCategories ();
            Assert.IsNull(testAllCategories);
            // We'll add new categories to test.
            Category testCategory1 = new Category(Globals.CategoryTypes.MainCategory, "Test Category 1", false, null);
            Category testCategory2 = new Category(Globals.CategoryTypes.MainCategory, "Test Category 2", false, null);
            testAllCategories = dbManager.Reader.GetAllCategories();
            Assert.IsNotNull (testAllCategories);
            Assert.AreEqual (2, testAllCategories.Length);
            dbManager.Writer.DeleteAllCategories();
        }

        [TestMethod()]
        public void GetAllKeywordsTest()
        {
            DatabaseManager dbManager = DatabaseManager.Instance;
            DatabaseInitialization.DatabaseInit();
            // Default keywords should exist after initialization
            string[]? testKeywords = dbManager.Reader.GetAllKeywords();
            Assert.IsNotNull (testKeywords);
            dbManager.Writer.DeleteAllKeywords ();
            testKeywords = dbManager.Reader.GetAllKeywords();
            Assert.IsNull (testKeywords);
            dbManager.Writer.AddKeyword("test Keyword");
            dbManager.Writer.AddKeyword("test Keyword");
            testKeywords = dbManager.Reader.GetAllKeywords();
            Assert.IsNotNull(testKeywords);
            Assert.AreEqual (2, testKeywords.Length);
            dbManager.Writer.DeleteAllKeywords();
        }

        [TestMethod()]
        public void GetCategoryTest()
        {
            DatabaseManager dbManager = DatabaseManager.Instance;
            DatabaseInitialization.DatabaseInit();
            dbManager.Writer.DeleteAllCategories ();
            Category testCategory1 = new Category(Globals.CategoryTypes.MainCategory, "Test Category", false, null);
            int testCategoryId = testCategory1.Id;
            Category testCategory2 =  dbManager.Reader.GetCategory(testCategoryId);
            Assert.AreEqual(testCategory1.Id, testCategory2.Id);
            Assert.AreEqual(testCategory1.Title, testCategory2.Title);
            dbManager.Writer.DeleteAllCategories();
        }

        [TestMethod()]
        public void GetTransactionTest()
        {
            DatabaseManager dbManager = DatabaseManager.Instance;
            DatabaseInitialization.DatabaseInit();
            Category testCategory = new Category(Globals.CategoryTypes.MainCategory, "Test Category", false, null);
            Transaction firstTransaction = new Transaction(DateTime.Now, 10.20m, Globals.TransactionTypes.Expense, true, null, testCategory, "Test Transaction 1", null, null);
            Transaction secondTransaction = new Transaction(DateTime.Now, 15.60m, Globals.TransactionTypes.Expense, true, null, testCategory, "Test Transaction 2", null, null);
            int firstTransactionId = firstTransaction.Id;
            int secondTransactionId = secondTransaction.Id;
            Transaction testFirstTransaction = dbManager.Reader.GetTransaction(firstTransactionId);
            Transaction testSecondTransaction = dbManager.Reader.GetTransaction(secondTransactionId);

            Assert.AreEqual(firstTransaction.Id, testFirstTransaction.Id);
            Assert.AreEqual(firstTransaction.Amount, testFirstTransaction.Amount);
            Assert.AreEqual(firstTransaction.IsImportant, testFirstTransaction.IsImportant);
            Assert.AreEqual(DateOnly.FromDateTime(firstTransaction.Date), DateOnly.FromDateTime(testFirstTransaction.Date));
            Assert.AreEqual(firstTransaction.Title, testFirstTransaction.Title);

            Assert.AreEqual(secondTransaction.Id, testSecondTransaction.Id);
            Assert.AreEqual(secondTransaction.Amount, testSecondTransaction.Amount);
            Assert.AreEqual(secondTransaction.IsImportant, testSecondTransaction.IsImportant);
            Assert.AreEqual(DateOnly.FromDateTime(secondTransaction.Date), DateOnly.FromDateTime(testSecondTransaction.Date));
            Assert.AreEqual(secondTransaction.Title, testSecondTransaction.Title);
            dbManager.Writer.DeleteAllTransactions();
            dbManager.Writer.DeleteAllCategories();
        }

        [TestMethod()]
        public void GetTransactionsTest()
        {
            // This one Gets Transactions from one datetime until another datetime
            // and Returns them in an ascending order, from the oldest to the most recent
            DatabaseManager dbManager = DatabaseManager.Instance;
            DatabaseInitialization.DatabaseInit();
            // In order to make sure this works, we'll clear the database first.
            dbManager.Writer.DeleteAllCategories();
            dbManager.Writer.DeleteAllTransactions();
            // now we can begin
            Category testCategory = new Category(Globals.CategoryTypes.MainCategory, "Test Category", false, null);
            DateTime testDateTime1 = DateTime.Parse("20/02/2025 12:05:20");
            DateTime testDateTime2 = DateTime.Parse("21/03/2025 12:05:20");
            DateTime testDateTime3 = DateTime.Parse("26/03/2025 12:05:20");
            DateTime testDateTime4 = DateTime.Parse("28/03/2025 12:05:20");

            Transaction firstTransaction = new Transaction(testDateTime1, 10.20m, Globals.TransactionTypes.Expense, true, null, testCategory, "Test Transaction 1", null, null);
            Transaction secondTransaction = new Transaction(testDateTime2, 15.60m, Globals.TransactionTypes.Expense, true, null, testCategory, "Test Transaction 2", null, null);
            Transaction thirdTransaction = new Transaction(testDateTime3, 18.10m, Globals.TransactionTypes.Expense, true, null, testCategory, "Test Transaction 3", null, null);
            Transaction fourthTransaction = new Transaction(testDateTime4, 19.70m, Globals.TransactionTypes.Expense, true, null, testCategory, "Test Transaction 4", null, null);
            // Between date 1 and date 3, there should be 3 transactions
            Transaction[]? testFoundTransactions = dbManager.Reader.GetTransactions(testDateTime1, testDateTime3);
            Assert.IsNotNull(testFoundTransactions);
            Assert.AreEqual(3, testFoundTransactions.Length);
            Assert.AreEqual(firstTransaction.Id, testFoundTransactions[0].Id);
            // between the date1 and date4, there should be 4 transactions.
            testFoundTransactions = dbManager.Reader.GetTransactions(testDateTime1, testDateTime4);
            Assert.AreEqual(4, testFoundTransactions.Length);
            Assert.AreEqual(fourthTransaction.Id, testFoundTransactions[3].Id);
            // between date1 and date2, there should be 2 transactions
            testFoundTransactions = dbManager.Reader.GetTransactions(testDateTime1, testDateTime2);
            Assert.AreEqual(2, testFoundTransactions.Length);
            Assert.AreEqual(secondTransaction.Id, testFoundTransactions[1].Id);
            // we can now delete all the transactions and categories.
            dbManager.Writer.DeleteAllCategories();
            dbManager.Writer.DeleteAllTransactions();
        }

        [TestMethod()]
        public void GetTransactionsTest1()
        {
            // I might delete this method (getting transactions between two Id numbers)
            // as it might be unreliable and unnecessary.
            Assert.IsTrue(true);
        }

        [TestMethod()]
        public void GetTransactionsTest2()
        {
            // Get transactions from a specific category.
            DatabaseManager dbManager = DatabaseManager.Instance;
            DatabaseInitialization.DatabaseInit();

            dbManager.Writer.DeleteAllTransactions();
            dbManager.Writer.DeleteAllCategories();

            Category testCategory1 = new Category(Globals.CategoryTypes.MainCategory, "Test Category 1", false, null);
            Category testCategory2 = new Category(Globals.CategoryTypes.MainCategory, "Test Category 2", false, null);
            // As there are no transactions for these category, it should return null.
            Transaction[]? foundTransactions1 = dbManager.Reader.GetTransactions(testCategory1);
            Transaction[]? foundTransactions2 = dbManager.Reader.GetTransactions(testCategory2);
            Assert.IsNull(foundTransactions1);
            Assert.IsNull(foundTransactions2);
            // We can add transactions now and test again.
            Transaction firstTransaction = new Transaction(DateTime.Now, 10.20m, Globals.TransactionTypes.Expense, true, null, testCategory1, "Test Transaction 1", null, null);
            Transaction secondTransaction = new Transaction(DateTime.Now, 15.60m, Globals.TransactionTypes.Expense, true, null, testCategory2, "Test Transaction 2", null, null);
            foundTransactions1 = dbManager.Reader.GetTransactions(testCategory1);
            foundTransactions2 = dbManager.Reader.GetTransactions(testCategory2);
            Assert.IsNotNull(foundTransactions1);
            Assert.AreEqual(1, foundTransactions1.Length);
            Assert.AreEqual(firstTransaction.Id, foundTransactions1[0].Id);
            Assert.IsNotNull(foundTransactions2);
            Assert.AreEqual(1, foundTransactions2.Length);
            Assert.AreEqual(secondTransaction.Id, foundTransactions2[0].Id);
            // We can now delete them all.
            dbManager.Writer.DeleteAllTransactions();
            dbManager.Writer.DeleteAllCategories();
        }

        [TestMethod()]
        public void GetTransactionsTest3()
        {
            // Gets transactions based on Keywords
            DatabaseManager dbManager = DatabaseManager.Instance;
            DatabaseInitialization.DatabaseInit();

            dbManager.Writer.DeleteAllTransactions();
            dbManager.Writer.DeleteAllCategories();
            dbManager.Writer.DeleteAllKeywords();

            string[]? testKeywords1 = { "Test"};
            string[]? testKeywords2 = { "New" };

            // There is no transaction with those keywords, so it should return null
            Transaction[]? foundTransactions1 = dbManager.Reader.GetTransactions("Test");
            Transaction[]? foundTransactions2 = dbManager.Reader.GetTransactions("New");
            Assert.IsNull(foundTransactions1);
            Assert.IsNull(foundTransactions2);

            Category testCategory = new Category(Globals.CategoryTypes.MainCategory, "Test Category", false, null);
            Transaction firstTransaction = new Transaction(DateTime.Now, 10.20m, Globals.TransactionTypes.Expense, true, testKeywords1, testCategory, "Test Transaction 1", null, null);
            Transaction secondTransaction = new Transaction(DateTime.Now, 15.60m, Globals.TransactionTypes.Expense, true, testKeywords1, testCategory, "Test Transaction 2", null, null);
            Transaction thirdTransaction = new Transaction(DateTime.Now, 18.05m, Globals.TransactionTypes.Expense, true, testKeywords2, testCategory, "Test Transaction 3", null, null);
            // this transaction should not come up in the results, as it does Not have any keywords.
            Transaction fourthTransaction = new Transaction(DateTime.Now, 8.30m, Globals.TransactionTypes.Expense, true, null, testCategory, "Test Transaction 4", null, null);
            // Now we can call the method again, the first keyword should return
            // an array with the length of 2 and the second keyword an array
            // with the length of 1
            foundTransactions1 = dbManager.Reader.GetTransactions("Test");
            foundTransactions2 = dbManager.Reader.GetTransactions("New");
            Assert.IsNotNull(foundTransactions1);
            Assert.IsNotNull(foundTransactions2);
            Assert.AreEqual(2, foundTransactions1.Length);
            Assert.AreEqual(1, foundTransactions2.Length);
            Assert.AreEqual(thirdTransaction.Id, foundTransactions2[0].Id);
            // We can now delete them all.
            dbManager.Writer.DeleteAllTransactions();
            dbManager.Writer.DeleteAllCategories();
        }

        [TestMethod()]
        public void GetTransactionsTest4()
        {
            // Gets transactions based on Transaction type (Expense, Debt, etc.)
            DatabaseManager dbManager = DatabaseManager.Instance;
            DatabaseInitialization.DatabaseInit();
            dbManager.Writer.DeleteAllTransactions();
            dbManager.Writer.DeleteAllCategories();
            // There are no transactions yet, so it should return null.
            Transaction[]? foundExpenses = dbManager.Reader.GetTransactions(Globals.TransactionTypes.Expense);
            Transaction[]? foundDebts = dbManager.Reader.GetTransactions(Globals.TransactionTypes.Debt);
            Transaction[]? foundOweds = dbManager.Reader.GetTransactions(Globals.TransactionTypes.Owed);
            Transaction[]? foundEarnings = dbManager.Reader.GetTransactions(Globals.TransactionTypes.Earning);
            Assert.IsNull(foundExpenses);
            Assert.IsNull(foundDebts);
            Assert.IsNull(foundOweds);
            Assert.IsNull(foundEarnings);
            Category testCategory = new Category(Globals.CategoryTypes.MainCategory, "Test Category", false, null);
            Transaction firstTransaction = new Transaction(DateTime.Now, 10.20m, Globals.TransactionTypes.Expense, true, null, testCategory, "Test Transaction 1", null, null);
            Transaction secondTransaction = new Transaction(DateTime.Now, 15.60m, Globals.TransactionTypes.Debt, true, null, testCategory, "Test Transaction 2", null, null);
            Transaction thirdTransaction = new Transaction(DateTime.Now, 18.05m, Globals.TransactionTypes.Owed, true, null, testCategory, "Test Transaction 3", null, null);
            Transaction fourthTransaction = new Transaction(DateTime.Now, 8.30m, Globals.TransactionTypes.Earning, true, null, testCategory, "Test Transaction 4", null, null);
            // Now each transaction type should have 1 Transaction.
            foundExpenses = dbManager.Reader.GetTransactions(Globals.TransactionTypes.Expense);
            foundDebts = dbManager.Reader.GetTransactions(Globals.TransactionTypes.Debt);
            foundOweds = dbManager.Reader.GetTransactions(Globals.TransactionTypes.Owed);
            foundEarnings = dbManager.Reader.GetTransactions(Globals.TransactionTypes.Earning);
            Assert.IsNotNull(foundExpenses);
            Assert.AreEqual(1, foundExpenses.Length);
            Assert.AreEqual(Globals.TransactionTypes.Expense, foundExpenses[0].TransactionType);
            Assert.AreEqual(firstTransaction.Id, foundExpenses[0].Id);
            Assert.IsNotNull(foundDebts);
            Assert.AreEqual(Globals.TransactionTypes.Debt, foundDebts[0].TransactionType);
            Assert.AreEqual(secondTransaction.Id, foundDebts[0].Id);
            Assert.IsNotNull(foundOweds);
            Assert.AreEqual(Globals.TransactionTypes.Owed, foundOweds[0].TransactionType);
            Assert.AreEqual(thirdTransaction.Id, foundOweds[0].Id);
            Assert.IsNotNull(fourthTransaction);
            Assert.AreEqual(Globals.TransactionTypes.Earning, foundEarnings[0].TransactionType);
            Assert.AreEqual(fourthTransaction.Id, foundEarnings[0].Id);
            // Now we can delete them all
            dbManager.Writer.DeleteAllTransactions();
            dbManager.Writer.DeleteAllCategories();
        }

        [TestMethod()]
        public void GetTransactionsTest5()
        {
            // Gets transactions of a specified amount
            DatabaseManager dbManager = DatabaseManager.Instance;
            DatabaseInitialization.DatabaseInit();
            dbManager.Writer.DeleteAllTransactions();
            dbManager.Writer.DeleteAllCategories();

            decimal testAmount1 = 9.99m;
            decimal testAmount2 = 18.05m;
            decimal testAmount3 = 8.30m;
            // There are no transactions so it should return null.
            Transaction[]? foundTransactions1 = dbManager.Reader.GetTransactions(testAmount1);
            Transaction[]? foundTransactions2 = dbManager.Reader.GetTransactions(testAmount2);
            Transaction[]? foundTransactions3 = dbManager.Reader.GetTransactions(testAmount3);

            Category testCategory = new Category(Globals.CategoryTypes.MainCategory, "Test Category", false, null);
            Transaction firstTransaction = new Transaction(DateTime.Now, testAmount1, Globals.TransactionTypes.Expense, true, null, testCategory, "Test Transaction 1", null, null);
            Transaction secondTransaction = new Transaction(DateTime.Now, testAmount1, Globals.TransactionTypes.Expense, true, null, testCategory, "Test Transaction 2", null, null);
            Transaction thirdTransaction = new Transaction(DateTime.Now, testAmount2, Globals.TransactionTypes.Expense, true, null, testCategory, "Test Transaction 3", null, null);
            Transaction fourthTransaction = new Transaction(DateTime.Now, testAmount3, Globals.TransactionTypes.Expense, true, null, testCategory, "Test Transaction 4", null, null);
            // The search for the first amount should return an array with 2 elements, the 2nd and
            //  third one, should return an array with only 1 element.
            foundTransactions1 = dbManager.Reader.GetTransactions(testAmount1);
            foundTransactions2 = dbManager.Reader.GetTransactions(testAmount2);
            foundTransactions3 = dbManager.Reader.GetTransactions(testAmount3);

            Assert.IsNotNull(foundTransactions1);
            Assert.AreEqual(2, foundTransactions1.Length);
            Assert.IsNotNull(foundTransactions2);
            Assert.AreEqual(1, foundTransactions2.Length);
            Assert.AreEqual(thirdTransaction.Id, foundTransactions2[0].Id);
            Assert.IsNotNull(foundTransactions3);
            Assert.AreEqual(1, foundTransactions3.Length);
            Assert.AreEqual(fourthTransaction.Id, foundTransactions3[0].Id);
            // Now we can delete them all.
            dbManager.Writer.DeleteAllTransactions();
            dbManager.Writer.DeleteAllCategories();
        }

        [TestMethod()]
        public void GetTransactionsTest6()
        {
            // Finds transaction with that have the specified string in their Title or Note
            // Can also not be an exact match.
            DatabaseManager dbManager = DatabaseManager.Instance;
            DatabaseInitialization.DatabaseInit();
            dbManager.Writer.DeleteAllTransactions();
            dbManager.Writer.DeleteAllCategories();

            string wordToSearch = "test";
            Transaction[]? foundTransactions = dbManager.Reader.GetTransactions("", false);
            Assert.IsNull(foundTransactions);

            Category testCategory = new Category(Globals.CategoryTypes.MainCategory, "Test Category", false, null);

            Transaction firstTransaction = new Transaction(DateTime.Now, 10.20m, Globals.TransactionTypes.Expense, true, null, testCategory, "Test Transaction 1", null, null);
            Transaction secondTransaction = new Transaction(DateTime.Now, 8.30m, Globals.TransactionTypes.Expense, true, null, testCategory, "Transaction 2", null, null);
            Transaction thirdTransaction = new Transaction(DateTime.Now, 4.18m, Globals.TransactionTypes.Expense, true, null, testCategory, "Transaction Testamony 3", null, null);
            Transaction fourthTransaction = new Transaction(DateTime.Now, 75m, Globals.TransactionTypes.Expense, true, null, testCategory, null, "Test 4", null);
            // It should find 3 (all except the 2nd transaction)
            foundTransactions = dbManager.Reader.GetTransactions(wordToSearch, false);
            Assert.IsNotNull(foundTransactions);
            Assert.IsTrue(foundTransactions.Any());
            Assert.AreEqual(3, foundTransactions.Length);

            // We can delete them all now
            dbManager.Writer.DeleteAllTransactions();
            dbManager.Writer.DeleteAllCategories();
        }

        [TestMethod()]
        public void CategoryExistsTest()
        {
            // Finds a category with the provided name
            DatabaseManager dbManager = DatabaseManager.Instance;
            DatabaseInitialization.DatabaseInit();
            dbManager.Writer.DeleteAllTransactions();
            dbManager.Writer.DeleteAllCategories();
            string categoryToSearch = "Test Category";
            // No category exists so it should return false
            bool testItExists = dbManager.Reader.CategoryExists(categoryToSearch);
            Assert.IsFalse(testItExists);
            Category testCategory1 = new Category(Globals.CategoryTypes.MainCategory , "Test Category 1", false, null);
            Category testCategory2 = new Category(Globals.CategoryTypes.MainCategory, "Test Category 2", false, null);
            Category testCategory3 = new Category(Globals.CategoryTypes.MainCategory, "Test Category", false, null);
            // It should now match 1 category
            testItExists = dbManager.Reader.CategoryExists(categoryToSearch);
            Assert.IsTrue(testItExists);
            // ***

            // We can delete them now
            dbManager.Writer.DeleteAllTransactions();
            dbManager.Writer.DeleteAllCategories();
        }

        [TestMethod()]
        public void KeywordExistsTest()
        {
            DatabaseManager dbManager = DatabaseManager.Instance;
            DatabaseInitialization.DatabaseInit();
            dbManager.Writer.DeleteAllTransactions();
            dbManager.Writer.DeleteAllCategories();
            dbManager.Writer.DeleteAllKeywords();

            string keywordToSeach = "Salary";
            bool testItExists = dbManager.Reader.KeywordExists(keywordToSeach);
            Assert.IsFalse(testItExists);
            dbManager.Writer.AddKeyword(keywordToSeach);
            testItExists = dbManager.Reader.KeywordExists(keywordToSeach);
            Assert.IsTrue(testItExists);
            //
            dbManager.Writer.DeleteAllTransactions();
            dbManager.Writer.DeleteAllCategories();
        }
    }
}
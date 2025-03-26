using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTrackerLibrary
{
    /// <summary>
    /// The class that contains methods needed to set up the required components of the program.
    /// </summary>
    public static class LibraryInitialization
    {
        /// <summary>
        /// Sets the foundations of the application by creating the required parts and setting 
        /// up the functions (*Should include some other parts in the code as well*).
        /// </summary>
        public static void Initialize()
        {
            DatabaseInitialization.DatabaseInit();
            CreateCategories();
            /*
            if (!DatabaseManager.DatabaseReader.HasCategories())
            {
                
            }
            */
            CreateKeywords();
        }

        /// <summary>
        /// Creates the default Categories.
        /// </summary>
        private static void CreateCategories ()
        {
            Category billCategory = new Category(Globals.CategoryTypes.MainCategory, "Bills", true, null);
            Category rentCategory = new Category(Globals.CategoryTypes.SubCategory, "Rent", true, null);
            Category electricityCategory = new Category(Globals.CategoryTypes.SubCategory, "Electricity", true, null);
            Category waterCategory = new Category(Globals.CategoryTypes.SubCategory, "Water", true, null);
            Category gasCategory = new Category(Globals.CategoryTypes.SubCategory, "Gas", true, null);
            Category internetCategory = new Category(Globals.CategoryTypes.SubCategory, "Internet", true, null);

            Category insuranceCategory = new Category(Globals.CategoryTypes.MainCategory, "Insurance", true, null);
            Category Categormedicaly = new Category(Globals.CategoryTypes.MainCategory, "Medical", true, null);

            Category groceryCategory = new Category (Globals.CategoryTypes.MainCategory, "Groceries", true, null);
            Category reweSubCategory = new Category(Globals.CategoryTypes.SubCategory, "Rewe", true, null);
            Category aldiSubCategory = new Category(Globals.CategoryTypes.SubCategory, "Aldi", true, null);
            Category pennySubCategory = new Category(Globals.CategoryTypes.SubCategory, "Penny", true, null);

            Category skinCategory = new Category(Globals.CategoryTypes.MainCategory, "Skincare and Makeup", true, null);
            Category foodAndDrinksCategory = new Category(Globals.CategoryTypes.MainCategory, "Food and Drinks", true, null);
            Category clothesCategory = new Category(Globals.CategoryTypes.MainCategory, "Clothes", true, null);

            Category transportationCategory = new Category(Globals.CategoryTypes.MainCategory, "Transportation", true, null);
            Category taxiCategory = new Category(Globals.CategoryTypes.SubCategory, "Taxi", true, null);
            Category trainCategory = new Category(Globals.CategoryTypes.SubCategory, "Train / Bus", true, null);

            Category entertainmentCategory = new Category(Globals.CategoryTypes.MainCategory, "Entertainments", true, null);
            Category checkCategory = new Category(Globals.CategoryTypes.MainCategory, "Checks", true, null);
            Category subscriptionCategory = new Category(Globals.CategoryTypes.MainCategory, "Subscription", true, null);
            Category miscCategory = new Category(Globals.CategoryTypes.MainCategory, "Miscellaneous", true, null);
            Category importCategory = new Category(Globals.CategoryTypes.MainCategory, "Imported Expenses", true, null);
        }

        /// <summary>
        /// Creates some default Keywords (needs some work).
        /// </summary>
        private static void CreateKeywords()
        {
            DatabaseManager.DatabaseWriter.AddKeyword("Daily");
            DatabaseManager.DatabaseWriter.AddKeyword("Monthly");
            DatabaseManager.DatabaseWriter.AddKeyword("Yearly");
        }
    }
}

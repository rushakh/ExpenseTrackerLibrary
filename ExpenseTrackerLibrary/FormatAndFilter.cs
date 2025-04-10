using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTrackerLibrary
{
    /// <summary>
    /// Contains tools for checking if the strings used in various parts of the program contain strings or 
    /// characters that are not permitted.
    /// </summary>
    internal static class FormatAndFilter
    {
        private static string[] notAllowedForKeyword = {" ", ",", "@", "%", "\"" };
        private static string[] notAllowedForCategory = {"@", "#", "%", "\""};
        private static string[] notAllowedForTransaction = {"@", "#", "%", "\""};

        /// <summary>
        /// Checks if the string array contains valid strings to be used as keywords.
        /// </summary>
        /// <param name="keywords"></param>
        /// <returns></returns>
        internal static bool AreKeywordsAllowed(string[] keywords)
        {
            bool isAllowed = true;
            if (keywords.Length > 0)
            {
                foreach (string keyword in keywords)
                {
                    isAllowed = IsKeywordAllowed(keyword);
                    if (!isAllowed) { return false; }
                }
                return isAllowed;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Checks if the inputed string is allowed to be used as Keyword.
        /// </summary>
        /// <param name="newKeyword"></param>
        /// <returns></returns>
        internal static bool IsKeywordAllowed(string newKeyword)
        {
            if (newKeyword != string.Empty)
            {
                bool isAllowed = true;
                foreach (string notAllowed in notAllowedForKeyword)
                {
                    if (newKeyword.Contains(notAllowed))
                    {
                        isAllowed = false;
                        // To not go through it when it is not not needed, I'll just put the return here
                        return isAllowed;
                    }
                }
                return isAllowed;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Checks if the inputed string is allowed to be used as the title for Category. Returns 
        /// false if a category with that name already exists.
        /// </summary>
        /// <param name="newCategoryTitle"></param>
        /// <returns></returns>
        internal static bool IsCategoryAllowed(string newCategoryTitle)
        {           
            if (newCategoryTitle != string.Empty)
            {
                bool isAllowed = true;
                // Check for not permitted characters and strings
                foreach (string notAllowed in notAllowedForCategory)
                {
                    if (newCategoryTitle.Contains(notAllowed))
                    {
                        isAllowed = false;
                        return isAllowed;
                    }
                }
                // Then we Check if a category with that name already exists.
                if (Globals.Database.Reader.CategoryExists(newCategoryTitle))
                {
                    isAllowed = false;
                }
                return isAllowed;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Checks if the inputed string is allowed to be used for Transaction title.
        /// </summary>
        /// <param name="newTransactionTitle"></param>
        /// <returns></returns>
        internal static bool IsTransactionTitleAllowed (string newTransactionTitle)
        {
            if (newTransactionTitle != string.Empty)
            {
                bool isAllowed = true;
                foreach (string notAllowed in notAllowedForTransaction)
                {
                    if (newTransactionTitle.Contains(notAllowed))
                    {
                        isAllowed = false;
                        return isAllowed;
                    }
                }
                return isAllowed;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Adds # to the beginning of a keyword if it doesn't contain it.
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        internal static string AddHashToKeyword (string keyword)
        {          
            if (!keyword.StartsWith ("#"))
            {
                string newKeyword = "#" + keyword;
                return newKeyword;
            }
            else
            {
                return keyword;
            }
        }

        /// <summary>
        /// Adds # to the beginning of every keyword in a list if they do not have it.
        /// </summary>
        /// <param name="keywords"></param>
        /// <returns></returns>
        internal static string[] AddHashToKeywords(string[] keywords)
        {
            List<string> keywordsList = new List<string>();
            foreach (string keyword in keywords)
            {
                string readyKeyword = AddHashToKeyword(keyword);
                keywordsList.Add(readyKeyword);
            }
            return keywordsList.ToArray();
        }

    }
}

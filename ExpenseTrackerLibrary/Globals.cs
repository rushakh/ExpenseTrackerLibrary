using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTrackerLibrary
{
    public static class Globals
    {
        public enum TransactionTypes { Expense = 0, Debt = 1, Owed = 2, Earning = 3};
        public enum TransactionMeans { Cash, DebitCard, CreditCard, Wallet};
        public enum CategoryTypes { MainCategory = 0, SubCategory = 1};
        public enum ImportValues { TransactionType, Amount, DateAndTime, Title, Note};
        public static string applicationPath = System.IO.Directory.GetCurrentDirectory();
        public static CultureInfo cultureInfo = CultureInfo.GetCultureInfo("en-GB");
        public static string dateTimeFormat = "dd/mm/yyyy";
    }
}

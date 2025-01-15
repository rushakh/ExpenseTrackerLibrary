using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTrackerLibrary
{
    public static class Globals
    {
        public enum TransactionTypes { Expense, Earning, Owed, Debt};
        public enum TransactionMeans { Cash, DebitCard, CreditCard, Wallet};
        public enum CategoryTypes { MainCategory, SubCategory};

    }
}

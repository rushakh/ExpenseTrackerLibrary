using System.Linq;

namespace ExpenseTrackerLibrary
{
    /// <summary>
    /// The financial accounts (expenses, debts, oweds, earnings) for a specific period of time
    /// or simply using an array of Transactions.
    /// </summary>
    public class Accounts : IAccounts
    {
        private DateTime _beginning;
        private DateTime _end;
        private Transaction[]? _transactions;
        private float _expenseSum;
        private float _debtSum;
        private float _owedSum;
        private float _earningSum;

        public DateTime Beginning { get => _beginning; }
        public DateTime End { get => _end; }
        public Transaction[]? Transactions { get => _transactions; }       
        public float ExpenseSum { get => _expenseSum; }
        public float DebtSum { get => _debtSum; }
        public float OwedSum { get => _owedSum; }
        public float EarningSum { get => _earningSum; }

        /// <summary>
        /// Constructor for creating an object of type Accounts that contains the financial accounts
        /// of a specified period of time.
        /// </summary>
        /// <param name="beginning"></param>
        /// <param name="end"></param>
        public Accounts (DateTime beginning, DateTime end)
        {
            // ***
            // get the transactions from the database and populate _transactions[].
            CalculateAccounts();
        }

        /// <summary>
        /// Constructor for creating an object of type Accounts using a provided array of transactions.
        /// </summary>
        /// <param name="transactions"></param>
        public Accounts(Transaction[] transactions)
        {
            _transactions = transactions;
            CalculateAccounts();
        }

        public Transaction[]? Expenses ()
        {
            var tempExpenses = from transaction in _transactions
                               where transaction.TransactionType == Globals.TransactionTypes.Expense
                               select transaction;
            if (tempExpenses is null)
            {
                return null;
            }
            else
            {
                Transaction[]? expenses = tempExpenses.ToArray();
                return expenses;
            }
        }

        public Transaction[]? Debts ()
        {
            var tempDebts = from transaction in _transactions
                               where transaction.TransactionType == Globals.TransactionTypes.Debt
                               select transaction;
            if (tempDebts is null)
            {
                return null;
            }
            else
            {
                Transaction[]? debts = tempDebts.ToArray();
                return debts;
            }
        }

        public Transaction[]? Oweds ()
        {
            var tempOweds = from transaction in _transactions
                               where transaction.TransactionType == Globals.TransactionTypes.Owed
                               select transaction;
            if (tempOweds is null)
            {
                return null;
            }
            else
            {
                Transaction[]? oweds = tempOweds.ToArray();
                return oweds;
            }
        }

        public Transaction[]? Earnings ()
        {
            var tempEarnings = from transaction in _transactions
                               where transaction.TransactionType == Globals.TransactionTypes.Earning
                               select transaction;
            if (tempEarnings is null)
            {
                return null;
            }
            else
            {
                Transaction[]? earnings = tempEarnings.ToArray();
                return earnings;
            }
        }

        /// <summary>
        /// Adds up the Amount properties of the transactions in a Transaction[]. 
        /// </summary>
        /// <param name="chosenTransactions"></param>
        /// <returns></returns>
        private float Sum (Transaction[]? chosenTransactions)
        {
            float sum = 0;
            if (chosenTransactions is null)
            {
                _expenseSum = 0;
            }
            else
            {
                var amounts = from transaction in chosenTransactions
                              select transaction.Amount;
                sum = amounts.Aggregate((a, b) => a + b);
            }
            return sum;
        }

        /// <summary>
        /// This should be used by the constructor. It Populates the Sum properties.
        /// </summary>
        private void CalculateAccounts()
        {
            var expenses = Expenses();
            _expenseSum = Sum(expenses);
            var debts = Debts();
            _debtSum = Sum(debts);
            var oweds = Oweds();
            _owedSum = Sum(oweds);
            var earnings = Earnings();
            _earningSum = Sum(earnings);
        }
    }
}

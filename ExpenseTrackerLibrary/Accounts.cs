using System.Linq;
using System.Transactions;

namespace ExpenseTrackerLibrary
{
    /// <summary>
    /// The financial accounts (expenses, debts, oweds, earnings) for a specific period of time
    /// or simply using an array of Transactions.
    /// </summary>
    public class Accounts : IAccounts
    {
        private int _id;
        private DateTime _beginning;
        private DateTime _end;
        private Transaction[]? _transactions;
        private decimal _expenseSum;
        private decimal _debtSum;
        private decimal _owedSum;
        private decimal _earningSum;

        /// <inheritdoc/>
        public int Id { get => _id; }
        /// <inheritdoc/>
        public DateTime Beginning { get => _beginning; }
        /// <inheritdoc/>
        public DateTime End { get => _end; }
        /// <inheritdoc/>
        public Transaction[]? Transactions { get => _transactions; }
        /// <inheritdoc/>
        public decimal ExpenseSum { get => _expenseSum; }
        /// <inheritdoc/>
        public decimal DebtSum { get => _debtSum; }
        /// <inheritdoc/>
        public decimal OwedSum { get => _owedSum; }
        /// <inheritdoc/>
        public decimal EarningSum { get => _earningSum; }
        /// <summary>
        /// Constructor for creating an object of type Accounts that contains the financial accounts
        /// of a specified period of time.
        /// </summary>
        /// <param name="beginning"></param>
        /// <param name="end"></param>
        public Accounts (DateTime beginning, DateTime end)
        {            
            _beginning = beginning;
            _end = end;
            _transactions = Globals.Database.Reader.GetTransactions(beginning, end);
            CalculateAccounts();
            _id = Globals.Database.Writer.AddAccount(_beginning, _end, _expenseSum, _debtSum, _owedSum, _earningSum);
        }        
        /// <summary>
        /// Constructor for creating an object of type Accounts using a provided array of transactions.
        /// </summary>
        /// <param name="transactions"></param>
        public Accounts(Transaction[] transactions)
        {
            var orderTransactions = from transaction in transactions
                                    orderby transaction.Date ascending
                                    select transaction;
            _transactions = orderTransactions.ToArray();
            CalculateAccounts();
        }
        /// <summary>
        /// Constructor for loading an object of type Accounts that contains the financial accounts 
        /// of a specified period of time from the database. 
        /// </summary>
        /// <param name="accountsId"></param>
        /// <param name="beginning"></param>
        /// <param name="end"></param>
        internal Accounts(int accountsId, DateTime beginning, DateTime end, decimal expensesSum, decimal debtSum, decimal owedSum, decimal earningSum)
        {
            // Can either get it from the database, or retrieve it again to calculate the sums.
            // But either way, will have to retrieve the transactions --> *** Might change this design
            // GetAccounts (beginning, end);
            _id = accountsId;
            _beginning = beginning;
            _end = end;
            _expenseSum = expensesSum;
            _debtSum = debtSum;
            _owedSum = owedSum;
            _earningSum = earningSum;
            _transactions = Globals.Database.Reader.GetTransactions(beginning, end);
        }
        /// <inheritdoc/>
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
        /// <inheritdoc/>
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
        /// <inheritdoc/>
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
        /// <inheritdoc/>
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
        private decimal Sum (Transaction[]? chosenTransactions)
        {
            decimal sum = 0;
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

namespace ExpenseTrackerLibrary
{
    /// <summary>
    /// The financial accounts (expenses, debts, oweds, earnings) for a specific period of time.
    /// </summary>
    public class Accounts : IAccounts
    {
        private DateTime _beginning;
        private DateTime _end;
        private Transaction[]? _transactions;
        private int _expenseSum;
        private int _debtSum;
        private int _owedSum;
        private int _earningSum;

        public DateTime Beginning { get => _beginning; }

        public DateTime End { get => _end; }

        public Transaction[]? Transactions { get => _transactions; }

        public int ExpenseSum
        {
            get
            { 
                // Calculate and then set private property.
                return _expenseSum;
            }
        }

        public int DebtSum
        {
            get
            {
                // Calculate and then set private property.
                return _debtSum;
            }
        }

        public int OwedSum
        {
            get
            {
                // Calculate and then set private property.
                return _owedSum;
            }
        }

        public int EarningSum
        {
            get
            {
                // Calculate and then set private property.
                return _earningSum;
            }
        }

        /// <summary>
        /// Constructor for creating an object of type Accounts that contains the financial accounts of a specified period of time.
        /// </summary>
        /// <param name="beginning"></param>
        /// <param name="end"></param>
        public Accounts (DateTime beginning, DateTime end)
        {
            // get the transactions from the database and populate _transactions[].
        }

        public Transaction[]? Expenses ()
        {
            return null;
        }

        public Transaction[]? Debts ()
        {
            // Use the transaction type to sort them out.
            return null;
        }

        public Transaction[]? Oweds ()
        {
            // Use the transaction type to sort them out.
            return null;
        }

        public Transaction[]? Earnings ()
        {
            return null;
        }
    }
}

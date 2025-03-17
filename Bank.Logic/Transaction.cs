using Bank.Logic.Abstractions;

namespace Bank.Logic;

public class Transaction : ITransaction
{
    public TransactionType Type { get; set; }
    public double Amount { get; set; }
    public DateTime Date { get; set; }

    public Transaction(TransactionType type, double amount, DateTime date)
    {
        // Validate amount based on transaction type
/*         if ((type == TransactionType.Deposit || type == TransactionType.Interest) && amount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be positive for Deposit and Interest");
        }
        if ((type == TransactionType.Withdraw || type == TransactionType.Fee_Overdraft) && amount >= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be negative for Withdraw and Fee_Overdraft");
        }
        if (type == TransactionType.Unknown && amount != 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be zero for Unknown");
        } */

        // Assign properties after validation
        Type = type;
        Amount = amount;
        Date = date;
    }
}
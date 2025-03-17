using Bank.Logic.Abstractions;
using System.Collections.Generic;

namespace Bank.Logic;

public class Account : IAccount
{
    private AccountSettings _settings;
    private readonly List<ITransaction> _transactions = new();

    public AccountSettings Settings 
    { 
        get => _settings; 
        set => _settings = value ?? throw new ArgumentNullException(nameof(value)); 
    }

    public double GetBalance()
    {
        double balance = 0;
        foreach (var transaction in _transactions)
        {
            balance += transaction.Amount;
        }
        return balance;
    }

    public IReadOnlyList<ITransaction> GetTransactions()
    {
        return _transactions.AsReadOnly();
    }

    public bool TryAddTransaction(ITransaction transaction)
    {
        if (transaction == null)
            return false;

        // Disallow direct interest and overdraft fee transactions
        if (transaction.Type == TransactionType.Interest || 
            transaction.Type == TransactionType.Fee_Overdraft)
            return false;

        // Validate amount based on transaction type
        if (transaction.Type == TransactionType.Deposit && transaction.Amount <= 0)
            return false;
        
        if (transaction.Type == TransactionType.Withdraw && transaction.Amount >= 0)
            return false;

        // For withdrawals, check balance and handle overdraft
        if (transaction.Type == TransactionType.Withdraw)
        {
            double currentBalance = GetBalance();
            double withdrawalAmount = -1 * Math.Abs(transaction.Amount); // Convert to positive for comparison
            
            if (withdrawalAmount == 0)
                return false;

            if (currentBalance < withdrawalAmount)
            {
                // Apply overdraft fee if configured
                if (Settings?.OverdraftFee > 0)
                {
                    _transactions.Add(new Transaction(
                        TransactionType.Fee_Overdraft,
                        -Settings.OverdraftFee,
                        transaction.Date));
                }
            }
        }

        _transactions.Add(transaction);
        return true;
    }
}

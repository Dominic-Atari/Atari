using ConsoleApp1;
namespace ConsoleApp1;

public class SavingAccount : Account, IAnnualInterestRate
{
    public decimal InterestRate { get; set; }

    // accountNumber is unique per account, customerId identifies the owner
    public SavingAccount(string accountName, long customerId, long accountNumber, decimal balance, decimal interestRate)
        : base(accountName, customerId, accountNumber, balance)
    {
        InterestRate = interestRate;
    }

    public override bool AccountNumberValidation(long accountNumber)
    {
        return AccountNumber == accountNumber;
    }

    public override bool AccountIdValidation(long accountId)
    {
        return CustomerId == accountId;
    }
    // Deposit amount into this account
    public override decimal Deposit(decimal amount)
    {
        if (amount <= 0)
            return Balance;

        Balance += amount;
        return Balance;
    }

    // Withdraw amount; for Savings account, ensure at least 50% remains (optional rule)
    public override decimal Withdraw(decimal amount)
    {
        if (amount <= 0 || amount > Balance / 2)
        {
            return Balance;
        }
        return Balance -= amount;
    }

    // Calculate annual interest
    public decimal CalculateInterestRate()
    {
        return Balance * (InterestRate / 100);
    }

    public override string DisplayInfo()
    {
        return $"Savings Account #{AccountNumber}: {AccountName}, Balance: {Balance:C}, Annual Interest: {CalculateInterestRate():C}";
    }
}
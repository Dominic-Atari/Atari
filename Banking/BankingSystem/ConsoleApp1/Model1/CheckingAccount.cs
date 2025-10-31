namespace ConsoleApp1;

public class CheckingAccount : Account
{
    public decimal AnnualFee { get; set; }

    // accountNumber is unique per account, customerId identifies the owner
    public CheckingAccount(string accountName, long customerId, long accountNumber, decimal balance, decimal annualFee)
        : base(accountName, customerId, accountNumber, balance)
    {
        AnnualFee = annualFee;
    }

    public override bool AccountNumberValidation(long accountNumber)
    {
        return AccountNumber == accountNumber;
    }

    public override bool AccountIdValidation(long customerId)
    {
        return CustomerId == customerId;
    }
    // Deposit amount into this account
    public override decimal Deposit(decimal amount)
    {
        if (amount <= 0)
            return Balance;

        Balance += amount;
        return Balance;
    }

    // Withdraw amount from this account
    public override decimal Withdraw(decimal amount)
    {
        if (amount <= 0 || amount > Balance)
            return Balance;

        Balance -= amount;
        return Balance;
    }

    public override string DisplayInfo()
    {
        return $"Checking Account #{AccountNumber}: {AccountName}, Balance: {Balance:C}, Annual Fee: {AnnualFee:C}";
    }
}
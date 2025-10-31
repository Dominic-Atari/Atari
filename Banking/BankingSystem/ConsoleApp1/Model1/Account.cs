using ConsoleApp1;

public abstract class Account
{
    public string AccountName { get; set; }
    public long CustomerId { get; set; }
    public long AccountNumber {get; set; }
    public decimal Balance { get; set; }

    public Account(string accountName, long customerId, long accountNumber, decimal balance)
    {
        AccountName = accountName;
        CustomerId = customerId;
        AccountNumber = accountNumber;
        Balance = balance;
    }

    public abstract bool AccountNumberValidation(long accountNumber);
    public abstract bool AccountIdValidation(long accountId);
    public abstract decimal Deposit(decimal amount);
    public abstract decimal Withdraw(decimal amount);
    public abstract string DisplayInfo();
    
    public override string ToString()
    {
        return $"AccountName: {AccountName}, AccountNumber: {AccountNumber}, Balance: {Balance:C}";
    }
}
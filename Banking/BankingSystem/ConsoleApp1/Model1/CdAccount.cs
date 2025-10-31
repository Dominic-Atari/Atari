namespace ConsoleApp1;

public class CdAccount : Account
{
    public decimal InterestRate { get; set; }
    public decimal PenaltyRate { get; set; }
    public DateTime CreatedAt { get; set; }
    private DateTime ValidUntil { get; set; }

    public CdAccount( string accountName, long accountId, long accountNumber, decimal balance, decimal interestRate, decimal penaltyRate, DateTime createdAt, DateTime validUntil)
        : base(accountName, accountId, accountNumber, balance)
    {
        InterestRate = interestRate;
        PenaltyRate = penaltyRate;
        CreatedAt = createdAt;
        ValidUntil = validUntil;
    }

    public override bool AccountIdValidation(long customerId)
    {
        return CustomerId == customerId;
    }

    public override bool AccountNumberValidation(long accountNumber)
    {
        return AccountNumber == accountNumber;
    }

    public override decimal Deposit(decimal amount)
    {
        if (amount < 0)
            return 0;

        return Balance += amount;
    }

    public override decimal Withdraw(decimal withdrawedAmount)
    {
        if (withdrawedAmount == 0 || withdrawedAmount > Balance)
        {
            return 0;
        }
        //Early withdrawal penalty
        if (DateTime.UtcNow <= ValidUntil)
        {
             var calculatedPenalty = withdrawedAmount * InterestRate * (PenaltyRate / 12);
           return Balance -= withdrawedAmount + calculatedPenalty;
        }
        return Balance -= withdrawedAmount;
    }

    public decimal CalculateInterestRate()
    {
        return Balance * (InterestRate / 100);
    }
    public override string DisplayInfo() { return $"CD Account #{AccountNumber}: {AccountName}, Balance: {Balance:C}, " + $"Interest: {CalculateInterestRate():C}";
    }
}
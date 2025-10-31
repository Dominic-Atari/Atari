using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ConsoleApp1;
using ConsoleApp1.Services;


public class NumbersFact
{
    public static void Main()
    {
        CustomerRegistration customerRegistrationObj = new ConsoleApp1.Services.CustomerRegistration();
        List<Customer> customerObj = new List<Customer>();
        AccountMenuService accountMenuObj = new AccountMenuService();

        bool home = false;
        while (!home)
        {
            Console.WriteLine();
            Console.WriteLine("REGISTRATION PAGE");
            Console.WriteLine("1. Create a new account");
            Console.WriteLine("Q. Quit");

            string? choice = Console.ReadLine()?.ToUpper();
            if (choice == "Q")
            {
                home = true;
            }
            else if (choice == "1")
            {
                Customer newCustomer = customerRegistrationObj.RegisterCustomer(customerObj);
                customerObj.Add(newCustomer);

                // Let customer access their accounts
                accountMenuObj.ChoseAccountType(customerObj);
            }
            else
            {
                Console.WriteLine("Invalid input. Please try again.");
            }
        }
    }
}

    
   /*The week 4 competency challenge.
 
Create an application with an OOP design to help manage bank accounts. 
Three types of accounts need to be handled - a savings account, a checking account, and a CD account. 
All three types of accounts have common information including an account id, the type of account, and current balance. 
In addition, the savings account has an annual interest rate, the checking account has an annual fee, and the CD has an annual interest rate and a penalty for early withdrawal.
Two types of transactions need to be handled:
Deposit - A deposit will include the account id and the amount of the deposit (which must be > 0).   All three accounts handle a deposit the same way.  If the account exists, the balance is increased by the deposit amount.
Withdrawal - A withdrawal will include the account id and the amount of the withdrawal (which must be > 0).  Withdrawals are handled differently depending on the account type.
A savings account withdrawal is allowed as long as the account balance is greater than the withdrawal amount.
A checking account withdrawal is allowed but only up to 50% of the account balance.
A CD withdrawal is allowed but the early withdrawal penalty is applied so the balance needs to be greater than the withdrawal amount and the penalty combined.
Technical specifications for your application include:
Create an abstract class for an account.  Account id, balance and type are to be properties.  In addition to the constructors, include a deposit method, an abstract withdrawal method, and a useful toString.
Create three classes that inherit from the account class - one each for savings, checkings and CDs.  Create properties for the respective data attributes, implement the withdrawal method for each as appropriate, and override the toString as appropriate.
In addition, the savings and CD will implement a calculate annual interest method from an interface that will simply return the annual amount earned based on the current balance and interest rate.  Don't add this amount to the balance, but do report it on the toString for a saving or CD account.
Create and test your classes and methods with hard-coded test data so that there is a list of accounts of different types with different balances.
Then, provide the user the following options:
L - List all of the accounts in the list including the account id, balance, and account type and also as appropriate the interest rate, annual fee, and early penalty, and finally for interest-bearing accounts, the amount of annual interest given the current balance and interest rate
D - Perform a deposit transaction by getting an account number from the user and a deposit amount and if the account exists add the deposit amount to the balance
W - Perform a withdrawal transaction by getting an account number from the user and a withdrawal amount and if the account exists with enough of a balance, perform the withdrawal including any penalties
Q - Quit the transaction processing
 */ 
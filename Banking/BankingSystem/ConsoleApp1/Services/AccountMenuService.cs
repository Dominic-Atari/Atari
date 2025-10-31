using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleApp1;
namespace ConsoleApp1.Services
{
    public class AccountMenuService
    {
        // Choose account type for a customer
        public void ChoseAccountType(List<Customer> listOfCustomers )
{
    bool chosen = true;

    while (chosen)
    {
        
        Console.WriteLine("CHOOSE ACCOUNT TYPE PAGE");
        Console.WriteLine("Enter (C) for Checking Account");
        Console.WriteLine("Enter (S) for Saving Account");
        Console.WriteLine("Enter (CD) for CD Account");
        Console.WriteLine("Enter (B) to Go Back");
        Console.WriteLine("Enter (A) to List All Customers");
        Console.WriteLine("Enter (TT) to check All Accounts Balance");

        string? choice = Console.ReadLine()?.ToUpper();

        if (choice == "B")
        {
            Console.WriteLine("Returning to previous menu...");
            chosen = false;
        }
        else if (choice == "C" || choice == "S" || choice == "CD")
        {
            Console.Write("Enter your Customer ID: ");
            string? customerIdInput = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(customerIdInput) && long.TryParse(customerIdInput, out long customerId))
            {
                var currentCustomer = listOfCustomers.FirstOrDefault(c => c.CustomerId == customerId);

                if (currentCustomer == null)
                {
                    Console.WriteLine("Customer not found.");
                    continue;
                }

                Console.Write("Enter your Account Number: ");
                string? accountNumberInput = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(accountNumberInput) && long.TryParse(accountNumberInput, out long accountNumber))
                {
                    var currentAccountNumber = currentCustomer.Accounts.FirstOrDefault(a => a.AccountNumber == accountNumber);

                    if (currentAccountNumber == null)
                    {
                        Console.WriteLine("This account number does not belong to the customer.");
                        continue;
                    }

                    // Check if account type matches user’s choice (current Account Type)
                    bool accountNumberMatchAccountType = (choice == "C" && currentAccountNumber is CheckingAccount) ||
                                     (choice == "S" && currentAccountNumber is SavingAccount) ||
                                     (choice == "CD" && currentAccountNumber is CdAccount);

                    if (!accountNumberMatchAccountType)
                    {
                        Console.WriteLine("The account number does not match the chosen account type.");
                        continue;
                    }

                    // If everything matches, continue
                    AccountProcessing(currentCustomer, currentAccountNumber);
                }
                else
                {
                    Console.WriteLine("Invalid Account Number.");
                }
            }
            else
            {
                Console.WriteLine("Invalid Customer ID.");
            }
        }
        else if (choice == "A")
        {
            ListAllUsers(listOfCustomers);
        }
        else if (choice == "TT")
        {
            Console.Write("Enter your Customer ID: ");
            string? customerIdInput = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(customerIdInput) && long.TryParse(customerIdInput, out long customerId))
            {
                var currentCustomer = listOfCustomers.FirstOrDefault(c => c.CustomerId == customerId);

                if (currentCustomer != null)
                {
                    SumBalance(currentCustomer.Accounts);
                }
                else
                {
                    Console.WriteLine("Customer not found.");
                }
            }
            else
            {
                Console.WriteLine("Invalid Customer ID.");
            }
        }



       
    }
}

        public static bool TieAccountIdWithAccountNumber(Account account, Customer customer)
        {
            // Rule: Account number must be generated from CustomerId * 10 + offset
            long expectedNumber = account switch
            {
                CheckingAccount => (customer.CustomerId * 100) + 1,
                SavingAccount   => (customer.CustomerId * 100) + 2,
                CdAccount       => (customer.CustomerId * 100) + 3,
                _               => 0
            };

            if (account.AccountNumber == expectedNumber)
            {
                Console.WriteLine("Account is properly tied to this customer.");
                //  Here you can safely call AccountProcessing(customer)
                return true;
            }
            else
            {
                Console.WriteLine("Account and Customer ID do not match!");
                return false;
            }
        }

        // Process deposit, withdraw, balance
        private void AccountProcessing(Customer customer, Account account)
        {
            bool option1 = true;

            while (option1)
            {
                Console.WriteLine();
                Console.WriteLine("ACCOUNT MENU");
                Console.WriteLine("Enter (D) to deposit");
                Console.WriteLine("Enter (W) to withdraw");
                Console.WriteLine("Enter (T) to check balance");
                Console.WriteLine("Enter (B) to go back");

                string? option2 = Console.ReadLine()?.ToUpper();

                if (option2 == "B")
                {
                    option1 = false;
                }
                else if (option2 == "D" || option2 == "W")
                {
                    Console.Write("Enter amount: ");
                    string? amountInput = Console.ReadLine();

                    if (!string.IsNullOrWhiteSpace(amountInput) && decimal.TryParse(amountInput, out decimal amount) && amount > 0)
                    {
                        if (option2 == "D")
                        {
                            if (account is CdAccount cd)
                            {
                                account.Deposit(amount);
                                Console.WriteLine($"successfully deposited {amount:C} Current Balance: {account.Balance:C} \n Annual Interest: {cd.CalculateInterestRate():C}");
                            }

                            else if (account is SavingAccount sa)
                            {
                                account.Deposit(amount);
                                Console.WriteLine($"Successfully deposited {amount:C} Current Balance: {account.Balance:C}  \n Annual Interest: {sa.CalculateInterestRate():C}");
                            }
                            else
                            {
                                account.Deposit(amount);
                                Console.WriteLine($"Successfully deposited {amount:C}. Current Balance: {account.Balance:C}");
                            }
                            
                        }
                        else // Withdraw
                        {
                            decimal beforeBalance = account.Balance;
                            account.Withdraw(amount);

                            if (account.Balance < beforeBalance)
                                Console.WriteLine($"Successfully withdrew {amount:C}. Current Balance: {account.Balance:C}");
                            else
                                Console.WriteLine("Withdrawal failed (exceeds limit or insufficient funds).");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid amount.");
                    }
                }
                else if (option2 == "T")
                {
                    Console.WriteLine(account);
                }
                else
                {
                    Console.WriteLine("Invalid option!");
                }
            }
        }


        // List all users
        public void ListAllUsers(List<Customer> listOfCustomers)
        {
            Console.WriteLine("==================");
            listOfCustomers.ForEach(customer =>
                Console.WriteLine($"First Name= {customer.FirstName}\nLast Name= {customer.LastName}\nID= {customer.CustomerId}\n"));
            Console.WriteLine("==================");
        }

        // Show balances for all accounts of the customer
        public void ShowBalance(List<Account> accounts)
        {
            if (!accounts.Any())
            {
                Console.WriteLine("No accounts exist.");
                return;
            }

            foreach (var acc in accounts)
            {
                Console.WriteLine(acc);

                if (acc is IAnnualInterestRate interestAcc)
                    Console.WriteLine($"Annual Interest: {interestAcc.CalculateInterestRate():C}");
            }
        }

        void SumBalance(List<Account> listOfAccounts)
        {
            if (!listOfAccounts.Any())
            {
                Console.WriteLine("No accounts exist.");
                return;
            }

            decimal total = listOfAccounts.Sum(acc => acc.Balance);
            Console.WriteLine($"Total Balance across all accounts: {total:C}");
           
        }
    
    
    
    }
}

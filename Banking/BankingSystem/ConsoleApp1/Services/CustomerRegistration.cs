using ConsoleApp1;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace ConsoleApp1.Services
{
    public class CustomerRegistration
    {
        public Customer RegisterCustomer(List<Customer> customers)
        {
            // Create a new customer object
            Customer customer = new Customer(string.Empty, string.Empty, 0);

            // First Name
            Console.Write("Enter First name: ");
            string firstName = Console.ReadLine() ?? string.Empty;
            while (string.IsNullOrWhiteSpace(firstName) || !Regex.IsMatch(firstName, @"^[A-Za-z]+$"))
            {
                Console.WriteLine("Invalid input. Please enter letters only.");
                Console.Write("Enter First name: ");
                firstName = Console.ReadLine() ?? string.Empty;
            }
            customer.FirstName = firstName;

            // Last Name
            Console.Write("Enter Last name: ");
            string lastName = Console.ReadLine() ?? string.Empty;
            while (string.IsNullOrWhiteSpace(lastName) || !Regex.IsMatch(lastName, @"^[A-Za-z]+$"))
            {
                Console.WriteLine("Invalid input. Please enter letters only.");
                Console.Write("Enter Last name: ");
                lastName = Console.ReadLine() ?? string.Empty;
            }
            customer.LastName = lastName;

            // Enter unique Customer ID
            long customerId;
            while (true)
            {
                Console.Write("Enter Your ID Number: ");
                string? input2 = Console.ReadLine();

                if (ValidateUserId(input2) && long.TryParse(input2, out customerId))
                {
                    if (customers.Any(c => c.CustomerId == customerId))
                    {
                        Console.WriteLine("Customer with this ID already exists. Try another ID.");
                    }
                    else
                    {
                        break; // valid and unique ID
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a numeric ID.");
                }
            }
            
            customer.CustomerId = customerId;
            
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine();
            Console.WriteLine($"SUCCESSFULLY REGISTERED!");
                             // $"\n    First Name: {char.ToUpper(customer.FirstName[0]) + customer.FirstName.Substring(1).ToLower()} " +
                             // $"\n    Last Name: {char.ToUpper(customer.LastName[0]) + customer.LastName.Substring(1).ToLower()}, " +
                             // $"\n    ID Number: {customer.CustomerId} ");
            Console.ResetColor();
            // Generate unique account numbers (example: simple incremental)
            long baseAccountNumber = customer.CustomerId * 100;

            // Checking account
            CheckingAccount checkingAccount = new CheckingAccount("Checking Account", customer.CustomerId,baseAccountNumber + 1,0m, 150m);
            Console.WriteLine("******************************");
            Console.WriteLine($"{checkingAccount.AccountName}: {checkingAccount.AccountNumber}");
            
            // Savings account with interest
            SavingAccount savingAccount = new SavingAccount("Saving Account",customer.CustomerId,baseAccountNumber + 2,0m,0.5m);
            // CD Account with penalty
            Console.WriteLine($"{savingAccount.AccountName}: {savingAccount.AccountNumber}.");
            
            CdAccount cdAccount = new CdAccount("CD Account", customer.CustomerId,baseAccountNumber + 3,0m,1.8m,10m,DateTime.Now,DateTime.Now.AddYears(1));
            Console.WriteLine($"Your {cdAccount.AccountName}: {cdAccount.AccountNumber}.");
            Console.WriteLine("******************************");
            
            // Store accounts inside customer object
            customer.Accounts = new List<Account> { checkingAccount, savingAccount, cdAccount };
            
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow; // just a color
            Console.WriteLine($"     WELCOME {char.ToUpper(firstName[0]) + firstName.Substring(1).ToLower()}");
            Console.ResetColor(); // end color
            return customer;
        }

        public bool ValidateUserId(string input)
        {
            return !string.IsNullOrWhiteSpace(input) &&
                   Regex.IsMatch(input, @"^[0-9]+$");
        }
       
    }
}

using System.Collections.Generic;

namespace ConsoleApp1
{
    public class Customer
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public long CustomerId { get; set; }
        public List<Account> Accounts { get; set; }
        

        public Customer(string firstName, string lastName, long customerId)
        {
            FirstName = firstName;
            LastName = lastName;
            CustomerId = customerId;
           // Accounts = new List<Account>(); // initialize empty list
        }
    }
}
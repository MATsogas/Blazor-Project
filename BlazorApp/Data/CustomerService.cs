using BlazorApp.Models;
using System;
using System.Linq;

namespace BlazorApp.Data
{
    public class CustomerService
    {
        private List<Customer> _customersSampleData;

        public CustomerService()
        {
            _customersSampleData = GetCustomers(30).ToList();
        }

        public Task<Customer[]> GetCustomersPaginated(int pageCount, int pageSize)
        {
            return Task.FromResult(
                _customersSampleData
                    .Skip((pageCount - 1) * pageSize)
                    .Take(pageSize)
                    .ToArray()
            );
        }

        public bool Insert(Customer customer)
        {
            try
            {
                _customersSampleData.Add(customer);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception thrown: {ex.Message} - Customer not added");
                return false;
            }
        }

        public bool Update(Customer customer)
        {
            var customerFound = _customersSampleData.FirstOrDefault(x => x == customer);
            if (customerFound == null)
            {
                Console.WriteLine("Customer could not be found - Update not performed!");
                return false;
            }

            customerFound = customer;
            return true;
        }

        public bool Upsert (Customer customer)
        {
            var customerFound = _customersSampleData.FirstOrDefault(x => x == customer);
            if (customerFound == null) { 
                return Insert(customer);
            }

            customerFound = customer;
            return true;
        }

        public bool Delete (Customer customer)
        {
            return _customersSampleData.Remove(customer);
        }

        public bool Delete (string id)
        {
            var customer = GetCustomerById(id);
            if (customer == null) {
                return false;
            }

            return _customersSampleData.Remove(customer);
        }

        public Customer GetCustomerById(string id)
        {
            var customer = _customersSampleData.FirstOrDefault(x => x.Id == id);
            if (customer == null)
            {
                Console.WriteLine($"Customer with id: {id} not found!");
                return null;
            }

            return customer;
        }

        #region Temporary Sample Data Creation
        private IEnumerable<Customer> GetCustomers(int customersToGenerate)
        {
            return Enumerable.Range(1, customersToGenerate).Select(index => CreateNewRandomCustomer(index.ToString()));
        }

        private char RandomCharacter()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return chars[Random.Shared.Next(0, chars.Length)];
        }

        private Customer CreateNewRandomCustomer(string id)
        {
            return new Customer
            {
                Id = id,
                Address = $"Sample Road {RandomCharacter()} {Random.Shared.Next(1, 300)}",
                City = $"Sample City {RandomCharacter()}",
                CompanyName = $"Company {RandomCharacter()}",
                ContactName = $"Dr. {RandomCharacter()}",
                Country = $"Country {RandomCharacter()}",
                Phone = string.Join("", Enumerable.Repeat(1, 10).Select(x => Random.Shared.Next(0, 9))),
                PostalCode = string.Join("", Enumerable.Repeat(1, 5).Select(x => Random.Shared.Next(0, 9))),
                Region = $"Region {RandomCharacter()}"
            };
        }
        #endregion
    }
}

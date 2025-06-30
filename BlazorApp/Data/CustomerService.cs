using BlazorApp.Models;
using System;
using System.Linq;

namespace BlazorApp.Data
{
    public class CustomerService
    {
        private IEnumerable<Customer> _customersSampleData;

        public CustomerService()
        {
            _customersSampleData = GetCustomers(30);
        }

        public Task<Customer[]> GetCustomersPaged(int pageCount, int pageSize)
        {
            return Task.FromResult(
                _customersSampleData
                    .Skip((pageCount - 1) * pageSize)
                    .Take(pageSize)
                    .ToArray()
            );
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

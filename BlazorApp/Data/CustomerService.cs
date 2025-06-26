using BlazorApp.Models;
using System;

namespace BlazorApp.Data
{
    public class CustomerService
    {
        public Task<Customer[]> GetCustomersPaged(int pageCount, int pageSize)
        {
            return Task.FromResult(Enumerable.Range(1, 5).Select(index => CreateNewRandomCustomer(index.ToString())).ToArray());
        }

        #region Temporary Sample Data Creation
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

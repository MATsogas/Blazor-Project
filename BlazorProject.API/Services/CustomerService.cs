using BlazorApp.Models;
using BlazorApp.API.Controllers;
using Microsoft.EntityFrameworkCore;
using BlazorApp.API.Models;

namespace BlazorApp.API.Services
{
    public class CustomerService
    {
        private readonly ILogger<CustomerController> _logger;
        private readonly ApplicationDbContext _context;

        private List<Customer> _customersSampleData;

        public CustomerService(
            ILogger<CustomerController> logger, 
            ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;

            _customersSampleData = GetCustomers(30).ToList();
        }

        public async Task<Customer[]> GetCustomersPaginated(int pageCount, int pageSize)
        {
            return _context.Customers
                .Skip((pageCount - 1) * pageSize)
                .Take(pageSize)
                .ToArray();
        }

        public async Task<bool> Insert(Customer customer)
        {
            try
            {
                _context.Customers.Add(customer);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding customer");
                return false;
            }
        }

        public async Task<bool> Update(Customer customer)
        {
            var customerFound = _context.Customers.FirstOrDefault(x => x == customer);
            if (customerFound == null)
            {
                _logger.LogWarning("Customer could not be found - Update not performed!");
                return false;
            }

            _context.Customers.Update(customer);
            _context.SaveChanges();
            return true;
        }

        public async Task<bool> Upsert (Customer customer)
        {
            var customerFound = _context.Customers.FirstOrDefault(x => x == customer);
            if (customerFound == null) { 
                return await Insert(customer);
            }

            _context.Customers.Update(customer);
            _context.SaveChanges();
            return true;
        }

        public async Task<bool> Delete (Customer customer)
        {
            _context.Customers.Remove(customer);
            _context.SaveChanges();
            return true;
        }

        public async Task<bool> Delete (string id)
        {
            var customer = await GetCustomerById(id);
            if (customer == null) {
                return false;
            }

            _context.Customers.Remove(customer);
            _context.SaveChanges();

            return true;
        }

        public async Task<Customer> GetCustomerById(string id)
        {
            var customer = _context.Customers.FirstOrDefault(x => x.Id == id);
            if (customer == null)
            {
                _logger.LogWarning($"Customer with id: {id} not found!");
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

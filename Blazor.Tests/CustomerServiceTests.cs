
using BlazorApp.Models;
using BlazorApp.API.Services;
using Shouldly;
using Microsoft.Extensions.Logging;
using BlazorApp.API.Controllers;
using BlazorApp.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;

namespace BlazorApp.Tests
{
    public class CustomerServiceTests
    {
        private readonly CustomerService _customerService;
        private readonly ApplicationDbContext _context;

        public CustomerServiceTests()
        {
            var logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<CustomerController>();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new ApplicationDbContext(options);

            //Seed test data
            _context.Customers.AddRange(GenerateCustomers(20));
            _context.SaveChanges();

            _customerService = new CustomerService(logger, _context);
        }

        [Fact]
        public async Task Insert_ShouldAddCustomer()
        {
            var newCustomer = new Customer { Id = "999", ContactName = "Test Contact" };

            var result = await _customerService.Insert(newCustomer);

            result.ShouldBeTrue();
            var inserted = await _customerService.GetCustomerById("999");
            inserted.ShouldNotBeNull();
            inserted.ShouldBe(newCustomer);
        }

        [Fact]
        public async Task GetCustomersPaginated_ShouldReturnCorrectPage()
        {
            var page = await _customerService.GetCustomersPaginated(2, 5);

            page.Length.ShouldBe(5);
            page.First().Id.ShouldBe("6");
        }

        [Fact]
        public async Task Update_ShouldUpdateExistingCustomer()
        {
            var customer = await _customerService.GetCustomerById("1");
            customer.ContactName = "Updated Name";

            var result = await _customerService.Update(customer);

            result.ShouldBeTrue();
            var updated = await _customerService.GetCustomerById("1");
            updated.ContactName.ShouldBe("Updated Name");
        }

        [Fact]
        public async Task Update_ShouldFailWhenCustomerNotFound()
        {
            var nonExistentCustomer = new Customer { Id = "999" };

            var result = await _customerService.Update(nonExistentCustomer);

            result.ShouldBeFalse();
        }

        [Fact]
        public async Task Upsert_ShouldInsertIfNotExists()
        {
            var newCustomer = new Customer { Id = "777", ContactName = "Upserted" };

            var result = await _customerService.Upsert(newCustomer);

            result.ShouldBeTrue();
            var upserted = await _customerService.GetCustomerById("777");
            upserted.ShouldNotBeNull();
            upserted.ShouldBe(newCustomer);
        }

        [Fact]
        public async Task Upsert_ShouldUpdateIfExists()
        {
            var customer = await _customerService.GetCustomerById("2");
            customer.ContactName = "Upserted Name";

            var result = await _customerService.Upsert(customer);

            result.ShouldBeTrue();
            var updatedCustomer = await _customerService.GetCustomerById("2");
            updatedCustomer.ShouldNotBeNull();
            updatedCustomer.ContactName.ShouldBe("Upserted Name");
        }

        [Fact]
        public async Task Delete_ByObject_ShouldRemoveCustomer()
        {
            var customer = await _customerService.GetCustomerById("3");

            var result = await _customerService.Delete(customer);

            result.ShouldBeTrue();
            customer = await _customerService.GetCustomerById("3");
            customer.ShouldBeNull();
        }

        [Fact]
        public async Task Delete_ById_ShouldRemoveCustomer()
        {
            var result = await _customerService.Delete("4");

            result.ShouldBeTrue();
            var customer = await _customerService.GetCustomerById("4");
            customer.ShouldBeNull();
        }

        [Fact]
        public async Task GetCustomerById_ShouldReturnCustomerIfExists()
        {
            var customer = await _customerService.GetCustomerById("5");

            customer.ShouldNotBeNull();
            customer.Id.ShouldBe("5");
        }

        [Fact]
        public async Task GetCustomerById_ShouldReturnNullIfNotExists()
        {
            var customer = await _customerService.GetCustomerById("not-real-id");

            customer.ShouldBeNull();
        }

        private IEnumerable<Customer> GenerateCustomers(int customersToGenerate)
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
    }
}

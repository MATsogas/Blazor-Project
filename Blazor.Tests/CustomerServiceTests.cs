
using BlazorApp.Models;
using BlazorApp.API.Services;
using Shouldly;
using Microsoft.Extensions.Logging;
using BlazorApp.API.Controllers;

namespace BlazorApp.Tests
{
    public class CustomerServiceTests
    {
        private readonly CustomerService _customerService;

        public CustomerServiceTests()
        {
            var logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<CustomerController>();
            _customerService = new CustomerService(logger);
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
    }
}

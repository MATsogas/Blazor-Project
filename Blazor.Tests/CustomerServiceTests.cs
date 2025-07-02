using BlazorApp.Data;
using BlazorApp.Models;
using Shouldly;

namespace BlazorApp.Tests
{
    public class CustomerServiceTests
    {
        private readonly CustomerService _customerService;

        public CustomerServiceTests()
        {
            _customerService = new CustomerService();
        }

        [Fact]
        public void Insert_ShouldAddCustomer()
        {
            var newCustomer = new Customer { Id = "999", ContactName = "Test Contact" };

            var result = _customerService.Insert(newCustomer);

            result.ShouldBeTrue();
            var inserted = _customerService.GetCustomerById("999");
            inserted.ShouldNotBeNull();
            inserted.ShouldBe(newCustomer);
        }

        [Fact]
        public async void GetCustomersPaginated_ShouldReturnCorrectPage()
        {
            var page = await _customerService.GetCustomersPaginated(2, 5);

            page.Length.ShouldBe(5);
            page.First().Id.ShouldBe("6");
        }

        [Fact]
        public void Update_ShouldUpdateExistingCustomer()
        {
            var customer = _customerService.GetCustomerById("1");
            customer.ContactName = "Updated Name";

            var result = _customerService.Update(customer);

            result.ShouldBeTrue();
            var updated = _customerService.GetCustomerById("1");
            updated.ContactName.ShouldBe("Updated Name");
        }

        [Fact]
        public void Update_ShouldFailWhenCustomerNotFound()
        {
            var nonExistentCustomer = new Customer { Id = "999" };

            var result = _customerService.Update(nonExistentCustomer);

            result.ShouldBeFalse();
        }

        [Fact]
        public void Upsert_ShouldInsertIfNotExists()
        {
            var newCustomer = new Customer { Id = "777", ContactName = "Upserted" };

            var result = _customerService.Upsert(newCustomer);

            result.ShouldBeTrue();
            var upserted = _customerService.GetCustomerById("777");
            upserted.ShouldNotBeNull();
            upserted.ShouldBe(newCustomer);
        }

        [Fact]
        public void Upsert_ShouldUpdateIfExists()
        {
            var customer = _customerService.GetCustomerById("2");
            customer.ContactName = "Upserted Name";

            var result = _customerService.Upsert(customer);

            result.ShouldBeTrue();
            _customerService.GetCustomerById("2").ContactName.ShouldBe("Upserted Name");
        }

        [Fact]
        public void Delete_ByObject_ShouldRemoveCustomer()
        {
            var customer = _customerService.GetCustomerById("3");

            var result = _customerService.Delete(customer);

            result.ShouldBeTrue();
            _customerService.GetCustomerById("3").ShouldBeNull();
        }

        [Fact]
        public void Delete_ById_ShouldRemoveCustomer()
        {
            var result = _customerService.Delete("4");

            result.ShouldBeTrue();
            _customerService.GetCustomerById("4").ShouldBeNull();
        }

        [Fact]
        public void GetCustomerById_ShouldReturnCustomerIfExists()
        {
            var customer = _customerService.GetCustomerById("5");

            customer.ShouldNotBeNull();
            customer.Id.ShouldBe("5");
        }

        [Fact]
        public void GetCustomerById_ShouldReturnNullIfNotExists()
        {
            var customer = _customerService.GetCustomerById("not-real-id");

            customer.ShouldBeNull();
        }
    }
}

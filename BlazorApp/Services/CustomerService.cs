using BlazorApp.Models;

namespace BlazorApp.Services
{
    public class CustomerService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<CustomerService> _logger;

        public CustomerService(
            HttpClient httpClient,
            ILogger<CustomerService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<Customer[]> GetCustomersPaginated(int skip, int take)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<Customer[]>($"api/Customer/GetCustomersPaginated?pageCount={skip}&pageSize={take}");
                return response ?? [];
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching paginated customers");
            }
            return [];
        }
    }
}

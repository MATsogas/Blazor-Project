using BlazorApp.API.Services;
using BlazorApp.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BlazorApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ILogger<CustomerController> _logger;
        private readonly CustomerService _customerService;

        public CustomerController(
            ILogger<CustomerController> logger,
            CustomerService customerService)
        {
            _logger = logger;
            _customerService = customerService;
        }

        [HttpGet("GetCustomersPaginated")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCustomersPaginated(int pageCount, int pageSize)
        {
            var response = await _customerService.GetCustomersPaginated(pageCount, pageSize);

            if (response == null || !response.Any())
            {
                return NotFound("No customers found.");
            }

            return Ok(response);
        }

        [HttpGet("GetCustomerById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCustomerById(string id)
        {
            var response = await _customerService.GetCustomerById(id);

            if (response == null)
            {
                return NotFound($"Customer with ID {id} not found.");
            }

            return Ok(response);
        }

        [HttpPut("Insert")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Insert(Customer customer)
        {
            var response = await _customerService.Insert(customer);

            if (response == false)
            {
                return BadRequest($"Customer could not be inserted.");
            }

            return Ok(response);
        }

        [HttpPost("Delete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(Customer customer)
        {
            var response = await _customerService.Delete(customer);

            if (response == false)
            {
                return BadRequest($"Customer could not be deleted.");
            }

            return Ok(response);
        }

        [HttpPost("DeleteById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteById(string id)
        {
            var response = await _customerService.Delete(id);

            if (response == false)
            {
                return BadRequest($"Customer could not be deleted.");
            }

            return Ok(response);
        }

        [HttpPut("Update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(Customer customer)
        {
            var response = await _customerService.Update(customer);
            if (response == false)
            {
                return BadRequest($"Customer could not be updated.");
            }
            return Ok(response);
        }

        [HttpPost("Upsert")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Upsert(Customer customer)
        {
            var response = await _customerService.Upsert(customer);
            if (response == false)
            {
                return BadRequest($"Customer could not be upserted.");
            }
            return Ok(response);
        }
    }
}

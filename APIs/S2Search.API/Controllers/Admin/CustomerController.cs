using Microsoft.AspNetCore.Mvc;
using S2Search.Backend.Domain.Customer.Customer;
using S2Search.Backend.Services.Services.Admin.Customer.Interfaces.Repositories;

namespace S2Search.Backend.Controllers.Admin
{
    [Route("api/customers/{customerId}")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepo;
        private readonly ILogger _logger;

        public CustomerController(ICustomerRepository customerRepo,
                                  ILogger<CustomerController> logger)
        {
            _customerRepo = customerRepo ?? throw new ArgumentNullException(nameof(customerRepo));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Gets the Customer by customerId
        /// </summary>
        [HttpGet(Name = "GetCustomer")]
        [ProducesResponseType(typeof(CustomerIds), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CustomerIds>> GetCustomer(Guid customerId)
        {
            try
            {
                var result = await _customerRepo.GetCustomerById(customerId);

                if (result == null)
                {
                    return NotFound("Customer not found");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error on {nameof(GetCustomer)} | CustomerId: {customerId} | Message: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Gets the Customer and their Search Indexes (CustomerFull) by customerId
        /// </summary>
        [HttpGet("searchIndexes", Name = "GetCustomerAndSearchIndexes")]
        [ProducesResponseType(typeof(CustomerFull), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CustomerFull>> GetCustomerFull(Guid customerId)
        {
            try
            {
                var result = await _customerRepo.GetCustomerFull(customerId);

                if (result == null)
                {
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error on {nameof(GetCustomerFull)} | CustomerId: {customerId} | Message: {ex.Message}");
                throw;
            }
        }
    }
}

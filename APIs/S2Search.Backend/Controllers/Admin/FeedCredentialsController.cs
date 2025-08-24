using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using S2Search.Backend.Domain.Customer.AppSettings;
using S2Search.Backend.Domain.Customer.SearchResources.FeedCredentials;
using S2Search.Backend.Services.Services.Admin.Customer.Interfaces.Managers;

namespace S2Search.Backend.Controllers.Admin
{
    [Route("api/customers/{customerId}/searchindex/{searchIndexId}/feedcredentials")]
    [ApiController]
    public class FeedCredentialsController : ControllerBase
    {
        private readonly IFeedCredentialsManager _credentialsManager;
        private readonly SftpSettings _sftpSettings;
        private readonly ILogger _logger;

        public FeedCredentialsController(IFeedCredentialsManager credentialsManager,
                                         IOptions<SftpSettings> options,
                                         ILogger<FeedCredentialsController> logger)
        {
            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            _credentialsManager = credentialsManager ?? throw new ArgumentNullException(nameof(credentialsManager));
            _sftpSettings = options.Value;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        /// <summary>
        /// Sends a request to create a Feed user account
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="searchIndexId"></param>
        /// <param name="request"></param>
        [HttpPost("user", Name = "CreateUser")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateUser(Guid customerId, Guid searchIndexId, [FromBody] CreateUserRequest request)
        {
            try
            {
                bool userExists = await CheckUserExists(searchIndexId, request.Username);

                if (userExists)
                {
                    _logger.LogInformation($"Bad request on {nameof(CreateUser)} | CustomerId: {customerId} | Username '{request.Username}' already exists");
                    return BadRequest("Username already exists");
                }

                request.SearchIndexId = searchIndexId;
                await _credentialsManager.CreateUser(request);

                return AcceptedAtAction(nameof(GetCredentials),
                                        ControllerContext.ActionDescriptor.ControllerName,
                                        new { customerId, searchIndexId });
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Error on {nameof(CreateUser)} | CustomerId: {customerId} | SearchIndexId: {searchIndexId} | Message: {ex.Message}");
                throw;
            }
            
        }

        /// <summary>
        /// Sends a request to delete a Feed user account
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="searchIndexId"></param>
        /// <param name="username"></param>
        [HttpDelete("user/{username}", Name = "DeleteUser")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteUser(Guid customerId, Guid searchIndexId, string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return BadRequest($"{nameof(username)} must be provided");
            }

            var deleteUserRequest = new DeleteUserRequest()
            {
                SearchIndexId = searchIndexId,
                Username = username
            };

            try 
            {
                bool userExists = await CheckUserExists(searchIndexId, username);

                if (!userExists)
                {
                    _logger.LogInformation($"Not found result on {nameof(DeleteUser)} | CustomerId: {customerId} | Username '{username}' does not exist");
                    return NotFound("User does not exist");
                }

                await _credentialsManager.DeleteUser(deleteUserRequest);

                return Accepted();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Error on {nameof(DeleteUser)} | CustomerId: {customerId} | SearchIndexId: {searchIndexId} | Message: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Sends a request to update the status of a Feed user account.
        /// 'Status' being whether the user is enabled or disabled.
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="searchIndexId"></param>
        /// <param name="request"></param>
        [HttpPut("userStatus", Name = "UpdateUserStatus")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateUserStatus(Guid customerId, Guid searchIndexId, [FromBody] UpdateUserStatusRequest request)
        {
            try
            {
                bool userExists = await CheckUserExists(searchIndexId, request.Username);

                if (!userExists)
                {
                    _logger.LogInformation($"Not found result on {nameof(UpdateUserStatus)} | CustomerId: {customerId} | Username '{request.Username}' does not exist");
                    return NotFound("User does not exist");
                }

                request.SearchIndexId = searchIndexId;
                await _credentialsManager.UpdateUserStatus(request);

                return Accepted();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Error on {nameof(UpdateUserStatus)} | CustomerId: {customerId} | SearchIndexId: {searchIndexId} | Message: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Sends a request to update the password of a Feed user account.
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="searchIndexId"></param>
        /// <param name="request"></param>
        [HttpPut("userPassword", Name = "UpdateUserPassword")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateUserPassword(Guid customerId, Guid searchIndexId, [FromBody] UpdatePasswordRequest request)
        {
            try
            {
                bool userExists = await CheckUserExists(searchIndexId, request.Username);

                if (!userExists)
                {
                    _logger.LogInformation($"Not found result on {nameof(UpdateUserPassword)} | CustomerId: {customerId} | Username '{request.Username}' does not exist");
                    return NotFound("User does not exist");
                }

                request.SearchIndexId = searchIndexId;
                await _credentialsManager.UpdatePassword(request);

                return Accepted();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Error on {nameof(UpdateUserPassword)} | CustomerId: {customerId} | SearchIndexId: {searchIndexId} | Message: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Gets a FeedCredentials object by SearchIndexId.
        /// This contains the username, date created and date modified.
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="searchIndexId"></param>
        [HttpGet("credentials", Name = "GetCredentials")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCredentials(Guid customerId, Guid searchIndexId)
        {
            try
            {
                var credentials = await _credentialsManager.GetCredentials(searchIndexId);

                if (string.IsNullOrEmpty(credentials?.Username))
                {
                    _logger.LogInformation($"Not found result on {nameof(GetCredentials)} | CustomerId: {customerId} | Username '{credentials?.Username}' does not exist");
                    return NotFound("User does not exist");
                }

                credentials.SftpEndpoint = _sftpSettings.SftpEndpoint;

                return Ok(credentials);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Error on {nameof(GetCredentials)} | CustomerId: {customerId} | SearchIndexId: {searchIndexId} | Message: {ex.Message}");
                throw;
            }
        }

        private async Task<bool> CheckUserExists(Guid searchIndexId, string username)
        {
            return await _credentialsManager.CheckUserExists(searchIndexId, username);
        }
    }
}

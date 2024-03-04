using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PalletSyncApi.Classes;
using PalletSyncApi.Services;
using System.Security.Authentication;
namespace PalletSyncApi.Controllers
{
    [ApiController]
    [Route("Users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                return Ok(await _userService.GetAllUsersAsync());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] User user)
        {
            if (!ModelState.IsValid || string.IsNullOrEmpty(user.Id))
            {
                return BadRequest("Invalid object!");
            }

            try
            {
                await _userService.AddUserAsync(user);
                return StatusCode(201);
            }
            catch (Exception ex) when (ex is InvalidOperationException || ex is DbUpdateException)
            {

                var errorResponse = new ErrorResponse
                {
                    Title = "One or more validation errors occurred.",
                    Status = 400,
                    Errors = new Dictionary<string, string[]>
                    {
                        { "Id", new[] { $"User with Id {user.Id} already exists!" } }
                    }
                };

                return BadRequest(errorResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            try
            {
                var result = await _userService.GetUserByIdAsync(id);
                if (result != null)
                {
                    return Ok(result);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserById(string id)
        {
            try
            {
                bool userDeleted = await _userService.DeleteUserByIdAsync(id);
                if (userDeleted)
                {
                    return Ok();
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500);
            }
        }


        [HttpPut]
        public async Task<IActionResult> UpdateUserById(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var updateSuccessful = await _userService.UpdateUserAsync(user);
                if (updateSuccessful)
                {
                    return Ok();
                }
                return NotFound();
            }
            catch (Exception ex) when (ex is InvalidOperationException || ex is DbUpdateException)
            {

                var errorResponse = new ErrorResponse
                {
                    Title = "One or more validation errors occurred.",
                    Status = 400,
                    Errors = new Dictionary<string, string[]> { 
                        { "Exception Message", new[] { ex.Message } }
                    }
                };

                return BadRequest(errorResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpPost("Authenticate")]
        public async Task<IActionResult> AuthenticateUser(string userId, string passCode, bool requestFromAdmin = false)
        {
            if (!ModelState.IsValid || string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(passCode))
            {
                return BadRequest("Invalid object!");
            }

            try
            {
                var valid = await _userService.AuthenticateUserAsync(userId, passCode, requestFromAdmin);
                return Ok();
            }
            catch (UnauthorizedAccessException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidCredentialException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchUsers([FromQuery] UniversalSearchTerm query)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                return Ok(await _userService.SearchUsersAsync(query));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500);
            }
        }
    }
}

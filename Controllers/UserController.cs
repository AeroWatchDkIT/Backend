﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PalletSyncApi.Classes;
using PalletSyncApi.Services;
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
    }
}

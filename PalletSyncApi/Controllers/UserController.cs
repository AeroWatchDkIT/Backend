using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
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
        GeneralUtilities utils = new GeneralUtilities();

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        private async Task<IActionResult> UploadImage(IFormFile file, string imagePath)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("File is not selected or is empty.");
            }

            using (var stream = new FileStream(imagePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Ok($"File uploaded successfully");
        }

        [HttpGet("GetImage")]
        public async Task<IActionResult> GetImage(string userId)
        {
            var imagePath = await _userService.GetImagePath(userId);
            var imageData = await _userService.GetImage(imagePath);

            if (imageData == null)
            {
                return NotFound();
            }

            // Determine the content type based on the file extension
            var contentType = utils.GetContentType(imagePath);

            // Return the image file as a file stream
            return File(imageData, contentType);
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
        public async Task<IActionResult> AddUser([FromForm] User user, IFormFile image)
        {
            if (!ModelState.IsValid || string.IsNullOrEmpty(user.Id))
            {
                return BadRequest("Invalid object!");
            }

            try
            {
                var folderName = "wwwroot/ProfileImages";

                if (!Directory.Exists(folderName))
                {
                    Directory.CreateDirectory(folderName);
                }

                var imageName = user.Id + ".jpg";
                user.ImageFilePath = Path.Combine(folderName, imageName);
                await UploadImage(image, user.ImageFilePath);
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

        [HttpPut("PasswordReset")]
        public async Task<IActionResult> UpdateUserPassword(string id, string newPassword)
        {
            try
            {
                bool passwordUpdated = await _userService.UpdateUserPasswordAsync(id, newPassword);
                if (passwordUpdated)
                {
                    return Ok();
                }
                return NotFound("User id not found");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500);
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

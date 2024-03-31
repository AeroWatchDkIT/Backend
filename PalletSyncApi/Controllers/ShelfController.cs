using Microsoft.AspNetCore.Mvc;
using PalletSyncApi.Services;
using System.Text.Json;
using PalletSyncApi.Classes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

namespace PalletSyncApi.Controllers
{
    [ApiController]
    [Route("Shelves")]
    public class ShelfController : ControllerBase
    {
        private readonly IShelfService _shelfService;

        public ShelfController(IShelfService shelfService)
        {
            _shelfService = shelfService;
        }

        [HttpGet(Name = "Shelves")]
        public async Task<IActionResult> GetShelves()
        {
            try
            {
                return Ok(await _shelfService.GetAllShelvesAsync());
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetShelfById(string id)
        {
            try
            {
                var result = await _shelfService.GetShelfByIdAsync(id);
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

        [HttpPost(Name = "Shelves")]
        public async Task<IActionResult> AddShelf([FromBody] Shelf shelf)
        {
            if (!ModelState.IsValid || shelf == null || string.IsNullOrEmpty(shelf.Id))
            {
                return BadRequest("Invalid object!");
            }

            try
            {
                shelf.Pallet = null;
                shelf.PalletId = null; //We dont want these 2 set from the create endpoint
                await _shelfService.AddShelfAsync(shelf);
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
                        { "Id", new[] { ex.InnerException.Message } }
                    }
                    };
                    return BadRequest(errorResponse);
                
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpPut(Name = "Shelves")]
        public async Task<IActionResult> UpdateShelf(Shelf shelf)
        {
            try
            {
                shelf.Pallet = null; //Dont want this set from outside
                await _shelfService.UpdateShelfFrontendAsync(shelf, true); //We should have an if statement to determine if we should call Frontend version or Hardware version
                return Ok($"Shelf {shelf.Id} has been successfully updated");
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occured when trying to update shelf {shelf.Id}");
            }
        }

        [HttpDelete(Name = "Shelves")]
        public async Task<IActionResult> DeleteShelf(string shelfId)
        {
            try
            {
                await _shelfService.DeleteShelfAsync(shelfId);
                return Ok($"Shelf {shelfId} has been successfully deleted");
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occured when trying to delete pallet {shelfId}");
            }
        }
    }
}

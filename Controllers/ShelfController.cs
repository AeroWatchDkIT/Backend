using Microsoft.AspNetCore.Mvc;
using PalletSyncApi.Services;
using System.Text.Json;
using PalletSyncApi.Classes;

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

        [HttpPost(Name = "Shelves")]
        public async Task<IActionResult> AddShelf([FromBody] Shelf shelf)
        {
            if (!ModelState.IsValid || string.IsNullOrEmpty(shelf.Id))
            {
                return BadRequest("Invalid object!");
            }

            try
            {
                await _shelfService.AddShelfAsync(shelf);
                return StatusCode(201);
            }
            catch (InvalidOperationException)
            {
                return BadRequest($"Shelf with Id {shelf.Id} already exists!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPut(Name = "Shelves")]
        public async Task<IActionResult> UpdateShelf(Shelf shelf)
        {
            try
            {
                await _shelfService.UpdateShelfAsync(shelf);
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

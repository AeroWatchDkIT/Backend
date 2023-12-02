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
        public async Task<IActionResult> GetPallets()
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
            throw new NotImplementedException();
        }

        [HttpPut(Name = "Shelves")]
        public async Task<IActionResult> UpdateShelf(Shelf shelf)
        {
            throw new NotImplementedException();
        }

        [HttpDelete(Name = "Shelves")]
        public async Task<IActionResult> DeleteShelf(string shelfId)
        {
            throw new NotImplementedException();
        }
    }
}

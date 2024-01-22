using Microsoft.AspNetCore.Mvc;
using PalletSyncApi.Services;
using System.Text.Json;
using PalletSyncApi.Classes;
using PalletSyncApi.Enums;

namespace PalletSyncApi.Controllers
{
    [ApiController]
    [Route("Pallets")]
    public class PalletController : ControllerBase
    {
        private readonly IPalletService _palletService;

        public PalletController(IPalletService palletService)
        {
            _palletService = palletService;
        }

        [HttpGet(Name = "Pallets")]
        public async Task<IActionResult> GetPallets()
        {
            try
            {
                return Ok(await _palletService.GetAllPalletsAsync());
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

        [HttpPost(Name = "Pallets")]
        public async Task<IActionResult> AddPallet([FromBody] Pallet pallet)
        {
            if (!ModelState.IsValid || string.IsNullOrEmpty(pallet.Id))
            {
                return BadRequest("Invalid object!");
            }

            try
            {
                pallet.State = PalletState.New; // We dont want to allow any other state at this point, has to start as NEW
                await _palletService.AddPalletAsync(pallet);
                return StatusCode(201);
            }
            catch (InvalidOperationException)
            {
                return BadRequest($"Pallet with Id {pallet.Id} already exists!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPut(Name = "Pallets")]
        public async Task<IActionResult> UpdatePallet(Pallet pallet)
        {
            try
            {
                await _palletService.UpdatePalletAsync(pallet);
                return Ok($"Pallet {pallet.Id} has been successfully updated");
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occured when trying to update pallet {pallet.Id}");
            }
        }

        [HttpDelete(Name = "Pallets")]
        public async Task<IActionResult> DeletePallet(string palletId)
        {
            try
            {
                await _palletService.DeletePalletAsync(palletId);
                return Ok($"Pallet {palletId} has been successfully deleted");
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occured when trying to delete pallet {palletId}");
            }
        }
    }
}

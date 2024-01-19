using Microsoft.AspNetCore.Mvc;
using PalletSyncApi.Classes;
using PalletSyncApi.Enums;
using PalletSyncApi.Services;

namespace PalletSyncApi.Controllers
{
    [ApiController]
    [Route("PalletStatuses")]
    public class PalletStatusController : ControllerBase
    {
        private readonly IPalletStatusService _palletStatusService;
        public PalletStatusController(IPalletStatusService palletStatusService)
        {
            _palletStatusService = palletStatusService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPalletStatuses()
        {
            try
            {
                return Ok(await _palletStatusService.GetAllPalletStatusesAsync());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500);
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchPalletStatuses([FromQuery] UniversalSearchTerm query)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                return Ok(await _palletStatusService.SearchPalletStatusesAsync(query));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500);
            }
        }

    }
}

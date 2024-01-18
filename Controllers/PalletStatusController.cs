using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> PalletStatuses()
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

    }
}

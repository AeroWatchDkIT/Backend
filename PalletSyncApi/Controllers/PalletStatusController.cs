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
        public async Task<IActionResult> GetAllPalletStatuses([FromQuery] Filter? filterTerm)
        {
            try
            {
                if (filterTerm.HasValue == true)
                {
                    return Ok(await _palletStatusService.GetAllPalletStatusesAsync(filterTerm));
                }
                return Ok(await _palletStatusService.GetAllPalletStatusesAsync());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, ex);
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchPalletStatuses([FromQuery] UniversalSearchTerm query, [FromQuery] Filter? filterTerm)
        {
            try
            {
                if (filterTerm.HasValue == true)
                {
                    return Ok(await _palletStatusService.SearchPalletStatusesAsync(query.SearchTerm, filterTerm));
                }
                return Ok(await _palletStatusService.SearchPalletStatusesAsync(query.SearchTerm));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, ex);
            }
        }
    }
}

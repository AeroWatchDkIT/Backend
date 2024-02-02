using Microsoft.AspNetCore.Mvc;
using PalletSyncApi.Classes;
using PalletSyncApi.Services;

namespace PalletSyncApi.Controllers
{
    [ApiController]
    [Route("TrackingLogs")]
    public class PalletTackingLogController : ControllerBase
    {
        private readonly IPalletTrackingLogService _palletTrackingLogService = new PalletTrackingLogService();

        [HttpGet]
        public async Task<IActionResult> GetTrackingLogs()
        {
            try
            {
                return Ok(await _palletTrackingLogService.GetAllTrackingLogsAsync());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500);
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchTrackingLogs([FromQuery] UniversalSearchTerm query)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                return Ok(await _palletTrackingLogService.SearchTrackingLogsAsync(query));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500);
            }
        }
    }
}

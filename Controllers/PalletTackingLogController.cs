using Microsoft.AspNetCore.Mvc;
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
    }
}

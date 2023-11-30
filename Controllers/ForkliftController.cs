using Microsoft.AspNetCore.Mvc;
using PalletSyncApi.Services;
using System.Text.Json;

namespace PalletSyncApi.Controllers
{
    [ApiController]
    [Route("Forklifts")]
    public class ForkliftController : ControllerBase
    {

        private readonly IForkliftService _forkliftService;

        public ForkliftController(IForkliftService forkliftService)
        {
            _forkliftService = forkliftService;
        }


        [HttpGet(Name = "Forklifts")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var result = _forkliftService.GetAllForklifts();
                var jsonResult = JsonSerializer.Serialize(result);
                return Ok(jsonResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

        [HttpPost(Name = "Forklifts")]
        public async Task<IActionResult> Post()
        {
            return NotFound();
        }

        [HttpPut(Name = "Forklifts")]
        public async Task<IActionResult> Put()
        {
            return BadRequest();
        }

        [HttpDelete(Name = "Forklifts")]
        public async Task<IActionResult> Delete()
        {
            return Ok();
        }

    }
}

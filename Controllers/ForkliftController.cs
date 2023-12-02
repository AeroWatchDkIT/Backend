using Microsoft.AspNetCore.Mvc;
using PalletSyncApi.Services;
using System.Text.Json;
using PalletSyncApi.Classes;

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
        public async Task<IActionResult> GetForklifts()
        {
            try
            { 
                return Ok(await _forkliftService.GetAllForkliftsAsync());
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

        [HttpPost(Name = "Forklifts")]
        public async Task<IActionResult> AddForklift([FromBody] Forklift forklift)
        {
            if (!ModelState.IsValid || string.IsNullOrEmpty(forklift.Id))
            {
                return BadRequest("Invalid object!");
            }

            forklift = sanitiseForkliftInput(forklift);

            try {
                await _forkliftService.AddForkliftAsync(forklift);
                return StatusCode(201);
            } 
            catch(InvalidOperationException) {
                return BadRequest($"Forklift with Id {forklift.Id} already exists!");
            }
            catch (Exception ex){
                return BadRequest(ex);
            }
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

        private Forklift sanitiseForkliftInput(Forklift forklift)
        {
            forklift.LastPallet = null;
            forklift.LastPalletId = null;
            forklift.LastUser = null;
            forklift.LastUserId = null;
            return forklift;
        }

    }
}

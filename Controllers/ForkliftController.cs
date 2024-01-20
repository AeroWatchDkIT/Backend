using Microsoft.AspNetCore.Mvc;
using PalletSyncApi.Services;
using PalletSyncApi.Classes;
using Microsoft.EntityFrameworkCore;

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


        [HttpGet]
        public async Task<IActionResult> GetForklifts()
        {
            try
            { 
                return Ok(await _forkliftService.GetAllForkliftsAsync());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500);
            }
        }

        [HttpPost]
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
            catch(Exception ex) when (ex is InvalidOperationException || ex is DbUpdateException) {

                var errorResponse = new ErrorResponse
                {
                    Title = "One or more validation errors occurred.",
                    Status = 400,
                    Errors = new Dictionary<string, string[]>
                    {
                        { "Id", new[] { $"Forklift with Id {forklift.Id} already exists!" } }
                    }
                };

                return BadRequest(errorResponse);
            }
            catch (Exception ex){
                return StatusCode(500, ex);
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchForklifts([FromQuery] SearchForklift query)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                return Ok(await _forkliftService.SearchForkliftsAsync(query));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetForkliftById(string id)
        {
            try
            {
                var result = await _forkliftService.GetForkliftByIdAsync(id);
                if(result != null)
                {
                    return Ok(result);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500);
            }
            // Come back to this and figure out a decent way to validate the id
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateForkliftIdById([FromBody] Forklift forklift, string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var updateSuccessful = await _forkliftService.UpdateForkliftByIdAsync(forklift.Id, id);
                if (updateSuccessful)
                {
                    return Ok();
                }
                return NotFound();
            }
            catch (Exception ex) when (ex is InvalidOperationException || ex is DbUpdateException)
            {

                var errorResponse = new ErrorResponse
                {
                    Title = "One or more validation errors occurred.",
                    Status = 400,
                    Errors = new Dictionary<string, string[]>
                    {
                        { "Id", new[] { $"Forklift with Id {forklift.Id} already exists!" } }
                    }
                };

                return BadRequest(errorResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteForkliftById(string id)
        {
            try
            {
                bool forkliftDeleted = await _forkliftService.DeleteObjectByIdAsync(id);
                if (forkliftDeleted)
                {
                    return Ok();
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500);
            }
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

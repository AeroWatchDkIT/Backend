using Microsoft.AspNetCore.Mvc;
using PalletSyncApi.Services;
using Newtonsoft.Json.Linq;
using PalletSyncApi.Classes;
using PalletSyncApi.Context;

namespace PalletSyncApi.Controllers
{

    [ApiController]
    [Route("Interactions")]
    public class InteractionsController: ControllerBase
    {
        private readonly IPalletService _palletService = new PalletService();
        private readonly IShelfService _shelfService = new ShelfService();
        private readonly IPalletTrackingLogService _palletTrackingLogService = new PalletTrackingLogService();

        PalletSyncDbContext context = new PalletSyncDbContext();

        [HttpPost]
        public async Task<IActionResult> CompareTwoCodes([FromBody] CompareTwoCodesJson data)
        {
            try
            {
                bool palletFound = context.Pallets.Where(p => p.Id == data.Pallet.Id).FirstOrDefault() != null;
                bool shelfFound = context.Shelves.Where(s => s.Id == data.Shelf.Id).FirstOrDefault() != null;

                var forklift = context.Forklifts.Where(f => f.Id == data.ForkliftId).FirstOrDefault();

                if(!palletFound || !shelfFound)
                {
                    return NotFound($"The given pallet and/or shelf code does not appear in the database. Please rescan the codes or enter them manually");
                }

                if (forklift == null)
                {
                    return NotFound($"Forklift id {data.ForkliftId} could not be found in the database");
                }

                await _palletService.UpdatePalletAsync(data.Pallet, false);
                await _shelfService.UpdateShelfFrontendAsync(data.Shelf, false);
                await _palletTrackingLogService.AddPalletTrackingLogAsync(data);

                forklift.LastPalletId = data.Pallet.Id;
                forklift.LastUserId = data.UserId;

                await context.SaveChangesAsync();

                var palletCode = data.Pallet.Id.Substring(data.Pallet.Id.IndexOf("-") + 1);
                var shelfCode = data.Shelf.Id.Substring(data.Shelf.Id.IndexOf("-") + 1);

                if(palletCode == shelfCode)
                {
                    return Ok($"Pallet code and shelf code are matching. All is good");
                }
                else
                {
                    return ValidationProblem($"Pallet code and shelf code do not match");
                }

                //TODO: Add state "misplaced" for pallets that are placed on the incorrect shelf
                //TODO: Accuracy of code scan
            }
            catch(Exception ex)
            {
                return BadRequest($"Something unexpected went wrong: {ex.Message}");
            }
        }
    }
}

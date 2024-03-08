using Microsoft.AspNetCore.Mvc;
using PalletSyncApi.Services;
using Newtonsoft.Json.Linq;
using PalletSyncApi.Classes;
using PalletSyncApi.Context;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.EntityFrameworkCore;

namespace PalletSyncApi.Controllers
{

    [ApiController]
    [Route("Interactions")]
    public class InteractionsController : ControllerBase
    {
        private readonly IPalletService _palletService = new PalletService();
        private readonly IShelfService _shelfService = new ShelfService();
        private readonly IPalletTrackingLogService _palletTrackingLogService = new PalletTrackingLogService();
        private readonly IForkliftService _forkliftService = new ForkliftService();

        PalletSyncDbContext context = new PalletSyncDbContext();

        [HttpPost("TwoCodes")]
        public async Task<IActionResult> CompareTwoCodes([FromBody] CompareTwoCodesJson data)
        {
            try
            {
                bool palletFound = context.Pallets.Where(p => p.Id == data.Pallet.Id).FirstOrDefault() != null;
                bool shelfFound = context.Shelves.Where(s => s.Id == data.Shelf.Id).FirstOrDefault() != null;
                bool forkliftFound = context.Forklifts.Where(f => f.Id == data.ForkliftId).FirstOrDefault() != null;

                if (!palletFound || !shelfFound)
                {
                    return NotFound($"The given pallet and/or shelf code does not appear in the database. " +
                        $"Please rescan the codes or enter them manually. " +
                        $"If the codes appear correct and you are still encountering an error, please contact an administrator");
                }

                if (!forkliftFound)
                {
                    return NotFound($"Forklift id {data.ForkliftId} could not be found in the database. Please contact an administrator");
                }

                await _palletService.UpdatePalletAsync(data.Pallet, false);
                await _shelfService.UpdateShelfFrontendAsync(data.Shelf, false);
                await _palletTrackingLogService.AddPalletTrackingLogAsync(data);
                await _forkliftService.UpdateForkliftAfterScan(data.ForkliftId, data.Pallet.Id, data.UserId);

                var palletCode = data.Pallet.Id.Substring(data.Pallet.Id.IndexOf("-") + 1);
                var shelfCode = data.Shelf.Id.Substring(data.Shelf.Id.IndexOf("-") + 1);

                if (palletCode == shelfCode)
                {
                    return Ok($"Pallet code and shelf code are matching. All is good");
                }
                else
                {
                    var correctShelf = context.Shelves.Where(s => s.Id.Contains(palletCode)).FirstOrDefault();
                    if (correctShelf != null)
                        return ValidationProblem($"Pallet code and shelf codes {data.Pallet.Id} and {data.Shelf.Id} do not match. " +
                            $"This pallet code belongs in shelf {correctShelf.Id} which is located in {correctShelf.Location}");
                    else
                        return ValidationProblem($"Pallet code and shelf codes {data.Pallet.Id} and {data.Shelf.Id} do not match. " +
                            $"There is no matching shelf code for pallet code {data.Pallet.Id}. Please contact an administrator");
                }

                //TODO: Add state "misplaced" for pallets that are placed on the incorrect shelf
                //TODO: Accuracy of code scan
            }
            catch (Exception ex)
            {
                return BadRequest($"Something unexpected went wrong: {ex.Message}");
            }
        }

        [HttpPost("OneCode")]
        public async Task<IActionResult> DealWithDeployedPallet([FromBody] CompareTwoCodesJson data)
        {
            // 1) not too sure about the correctness of UpdateForkliftAfterScan setting the last user id, but that may be something to change later
            // 2) change to compare2codes schema to make shelf nullable needs to be analysed, the above endpoint seems to deal with it ok however
            // 3) making pallet's location field nullable would be nice but no idea what domino effect that could cause through rest of code

            // 4) we really should refactor both of these endpoints to make use of transactions to maintain the consistency of the database
            // transactions with efcore actually seem not too bad https://www.youtube.com/watch?v=25H84BXcr9M&ab_channel=MilanJovanovi%C4%87, but i think we would need to make PalletSyncDbContext
            // a singleton and pass it around via Dependency Injection for transactions to work, as we are currently just make new instances of PalletSyncDbContext wherever it is needed
            try
            {
                bool palletFound = context.Pallets.Where(p => p.Id == data.Pallet.Id).FirstOrDefault() != null;
                bool forkliftFound = context.Forklifts.Where(f => f.Id == data.ForkliftId).FirstOrDefault() != null;

                if (!palletFound)
                {
                    return NotFound($"The given pallet code does not appear in the database. " +
                        $"Please rescan the codes or enter them manually. " +
                        $"If the codes appear correct and you are still encountering an error, please contact an administrator");
                }
                if (!forkliftFound)
                {
                    return NotFound($"Forklift id {data.ForkliftId} could not be found in the database. Please contact an administrator");
                }

                bool pickingPalletOffFloor = await ValidateStateChangeForPalletCodeOnly(data.Pallet);

                if (pickingPalletOffFloor)
                    data.Pallet.Location = "";

                await _palletService.UpdatePalletAsync(data.Pallet, true);                                 
                await _forkliftService.UpdateForkliftAfterScan(data.ForkliftId, data.Pallet.Id, data.UserId);
                await _palletTrackingLogService.AddPalletTrackingLogAsync(data);

                if (pickingPalletOffFloor)
                    return Ok($"Pallet " + data.Pallet.Id + " picked up from location: " + data.Pallet.Location);
                else
                    return Ok($"Pallet " + data.Pallet.Id + " left at location: " + data.Pallet.Location);
            }
            catch (Exception ex)
            {
                return BadRequest($"Something unexpected went wrong: {ex.Message}");
            }
        }

        private async Task<bool> ValidateStateChangeForPalletCodeOnly(Pallet newPallet)
        {
            Pallet oldPallet = await context.Pallets.FirstOrDefaultAsync(p => p.Id == newPallet.Id);

            if (oldPallet.State == Enums.PalletState.Floor && !string.IsNullOrEmpty(oldPallet.Location)
                && !string.IsNullOrWhiteSpace(oldPallet.Location) && newPallet.State == Enums.PalletState.Fork
                && string.IsNullOrEmpty(newPallet.Location))
            {
                // This checks that our pallet starts off in a valid expected state (sitting on the floor somewhere)
                // And when the forklift picks it up, that the new state of the pallet is also valid and what we expect
                return true;
            }
            else if (oldPallet.State == Enums.PalletState.Fork && string.IsNullOrEmpty(oldPallet.Location)
                && string.IsNullOrWhiteSpace(oldPallet.Location) && newPallet.State == Enums.PalletState.Floor
                && !string.IsNullOrEmpty(newPallet.Location) && !string.IsNullOrWhiteSpace(newPallet.Location))
            {
                // This checks that our pallet starts off in a valid expected state (sitting on forklift)
                // And when the forklift tries to put it on the floor, that the new state of the pallet is also valid and what we expect
                return false;
            }
            else
            {
                throw new InvalidOperationException("the database is inconsistent or wrong instruction was sent by the forklift operator, please contact the admin!");
            }
        }
    }
}

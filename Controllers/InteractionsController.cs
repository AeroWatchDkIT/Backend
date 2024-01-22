using Microsoft.AspNetCore.Mvc;
using PalletSyncApi.Services;
using Newtonsoft.Json.Linq;
using PalletSyncApi.Classes;

namespace PalletSyncApi.Controllers
{
    [ApiController]
    [Route("Interactions")]
    public class InteractionsController
    {
        private readonly IForkliftService _forkliftService;
        private readonly IPalletService _palletService;
        private readonly IShelfService _shelfService;
        private readonly IUserService _userService;

        [HttpPost]
        public async Task<IActionResult> CompareTwoCodes([FromBody] JObject data)
        {
            try
            {
                Pallet pallet = data["palletData"].ToObject<Pallet>();
                Shelf shelf = data["shelfData"].ToObject<Shelf>();

                var timeOfInteraction = data["time"];
                var userId = data["userId"];
                var forkliftId = data["forkliftId"];

                //TODO: Check if pallet/shelf code exists in database. If not, return an error code

                //TODO: Add state "misplaced" for pallets that are placed on the incorrect shelf
                //TODO: Accuracy of code scan
            }
            catch
            {
                Console.WriteLine("Some bullshit happened");
            }

            return null;
        }
    }
}

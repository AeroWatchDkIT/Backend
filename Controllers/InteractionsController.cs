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

        [HttpPost]
        public async Task<IActionResult> CompareTwoCodes([FromBody] JObject data)
        {
            try
            {
                Pallet pallet = data["palletData"].ToObject<Pallet>();
                Shelf shelf = data["shelfData"].ToObject<Shelf>();

                //TODO: Add state "misplaced" for pallets that are placed on the incorrect shelf
            }
            catch
            {
                Console.WriteLine("Some bullshit happened");
            }

            return null;
        }
    }
}

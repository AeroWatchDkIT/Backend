using Microsoft.EntityFrameworkCore;
using PalletSyncApi.Context;
using PalletSyncApi.Classes;
using PalletSyncApi.Enums;


namespace PalletSyncApi.Services
{
    public class PalletStatusService: IPalletStatusService
    {
        PalletSyncDbContext context = new PalletSyncDbContext();

        public async Task<object> GetAllPalletStatusesAsync()
        {
            var dbQuery = context.Pallets.ToListAsync();
                          

            var palletStatuses = await dbQuery;
            var result = await CreatePalletStatusResult(palletStatuses);
            return result;
        }

        private async Task<object> CreatePalletStatusResult(List<Pallet> pallets)
        {
            List<PalletStatus> statuses = new List<PalletStatus>();

            foreach(var pallet in pallets)
            {
                PalletStatus currentPalletStatus;

                if (pallet.State.Equals(PalletState.Floor))
                {
                    // Think about this one, how we treat location, for example, is location always there or only there 
                    // When a pallet is placed on the floor?
                    currentPalletStatus = makeFloorPalletStatus(pallet);
                }
                else if (pallet.State.Equals(PalletState.Fork))
                {
                    currentPalletStatus = makeInTransitPalletStatus(pallet);
                }
                else if (pallet.State.Equals(PalletState.Shelf))
                {
                    currentPalletStatus = await makeShelvedPalletStatus(pallet);
                }
                else
                {
                    currentPalletStatus = makeMissingPalletStatus(pallet);
                }

                statuses.Add(currentPalletStatus);
            }
            return WrapListOfPalletStatuses(statuses);
        }


        private async Task<PalletStatus> makeShelvedPalletStatus(Pallet pallet)
        {
            PalletStatus palletStatus = preparePalletStatus(pallet);

            string? place = await context.Shelves
                .Where(shelf => shelf.PalletId == pallet.Id)
                .Select(shelf => shelf.Id)
                .FirstOrDefaultAsync();

            palletStatus.Place = place;
            return palletStatus;
        }




        private PalletStatus makeInTransitPalletStatus(Pallet pallet)
        {
            PalletStatus palletStatus = preparePalletStatus(pallet);
            // This will involve a join with users and forklifts table, can be done once the
            // forklift table has a field indicating that its currently handling a pallet
            palletStatus.Place = "Stevens Forklift";
            return palletStatus;
        }



        private PalletStatus makeMissingPalletStatus(Pallet pallet)
        {
            PalletStatus palletStatus = preparePalletStatus(pallet);
            palletStatus.Place = pallet.State.ToString();
            return palletStatus;
        }
        private PalletStatus makeFloorPalletStatus(Pallet pallet)
        {
            PalletStatus palletStatus = preparePalletStatus(pallet);
            palletStatus.Place = pallet.Location;
            return palletStatus;
        }

        private PalletStatus preparePalletStatus(Pallet pallet)
        {
            PalletStatus palletStatus = new PalletStatus();
            palletStatus.Name = pallet.Id;
            return palletStatus;
        }

        private object WrapListOfPalletStatuses(List<PalletStatus> palletStatuses)
        {
            var palletStatusWrapper = new
            {
                palletStatuses
            };
            return palletStatusWrapper;
        }
    }
}

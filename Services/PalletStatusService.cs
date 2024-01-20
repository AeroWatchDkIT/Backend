using Microsoft.EntityFrameworkCore;
using PalletSyncApi.Context;
using PalletSyncApi.Classes;
using PalletSyncApi.Enums;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;


namespace PalletSyncApi.Services
{
    public class PalletStatusService: IPalletStatusService
    {
        PalletSyncDbContext context = new PalletSyncDbContext();
        GeneralUtilities util = new GeneralUtilities();

        public async Task<object> GetAllPalletStatusesAsync()
        {
            context = util.RemakeContext(context);
            var dbQuery = context.Pallets.ToListAsync();
            var palletStatuses = await dbQuery;
            var result = await CreatePalletStatusResult(palletStatuses);
            return result;
        }

        public async Task<object> GetAllPalletStatusesAsync(Filter? filterTerm)
        {
            context = util.RemakeContext(context);
            var dbQuery = SelectAppropriateQuery(filterTerm);

            /*var dbQuery = from user in context.Users
                          join forklift in context.Forklifts on user.Id equals forklift.LastUserId
                          join pallet in context.Pallets on forklift.LastPalletId equals pallet.Id
                          join shelf in context.Shelves on pallet.Id equals shelf.PalletId
                          select new
                          {
                              palletId = pallet.Id,
                              shelfId = shelf.Id,
                              shelfChild = shelf.PalletId
                              //lastUserOnForklift = user != null ? $"{user.FirstName} {user.LastName}" : null,
                          };*/


            

            //dbQuery = dbQuery.Where(result => result.palletId != result.shelfChild);

            var palletStatuses = await dbQuery.ToListAsync();
            var result = await CreatePalletStatusResult(palletStatuses);
            return result;
        }

         private IQueryable<Pallet> SelectAppropriateQuery(Filter? filterTerm, UniversalSearchTerm searchTerm = null)
        {
            switch (filterTerm)
            {
                case Filter.Misplaced:
                    {
                        //query = query.Where(pallet => pallet.Id.ToString().Contains(x));

                        //query = query.Where(shelf => shelf.PalletId == pallet.Id)
                        //.Select(shelf => shelf.Id)
                        //.FirstOrDefaultAsync();

                        //.Where(shelf => shelf.PalletId == pallet.Id)
                        //.Select(shelf => shelf.Id)
                        //.FirstOrDefaultAsync();
                        return GetAllMisplacedPallets(filterTerm, searchTerm = null);
                    }
                /*case Filter.InPlace:
                    {
                        break;
                    }
                case Filter.OnFloor:
                    {
                        break;
                    }
                case Filter.Missing:
                    {
                        break;
                    }
                case Filter.InTransit:
                    {
                        break;
                    }*/
            }
            return null;
        }

        private IQueryable<Pallet> GetAllMisplacedPallets(Filter? filterTerm, UniversalSearchTerm searchTerm = null)
        {
            /*var dbQuery = from pallet in context.Pallets
                          join shelf in context.Shelves on pallet.Id.Substring(2) equals shelf.PalletId.Substring(2)
                          where pallet.Id != shelf.PalletId
                          && ((searchTerm != null && !string.IsNullOrEmpty(searchTerm.SearchTerm)) ? (pallet.Id.ToString().Contains(searchTerm.SearchTerm)) : true)
                          select pallet;*/

            var dbQuery = from pallet in context.Pallets
                          join shelf in context.Shelves on pallet.Id equals shelf.PalletId
                          where pallet.Id.Substring(2) != shelf.Id.Substring(2)
                          select pallet;

            return dbQuery;
        }


        public async Task<object> SearchPalletStatusesAsync(UniversalSearchTerm query)
        {
            context = util.RemakeContext(context);

            var dbQuery = context.Pallets
                .Where(pallet => pallet.Id.ToString().Contains(query.SearchTerm))
                .ToListAsync();

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

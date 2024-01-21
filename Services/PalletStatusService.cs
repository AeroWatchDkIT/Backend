using Microsoft.EntityFrameworkCore;
using PalletSyncApi.Context;
using PalletSyncApi.Classes;
using PalletSyncApi.Enums;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.AspNetCore.Mvc.Filters;


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

            var palletStatuses = await dbQuery.ToListAsync();
            var result = await CreatePalletStatusResult(palletStatuses);
            return result;
        }

         private IQueryable<Pallet> SelectAppropriateQuery(Filter? filterTerm, string searchTerm = "")
        {
            switch (filterTerm)
            {
                case Filter.Misplaced:
                    {
                        return GetAllMisplacedPallets(searchTerm);
                    }
                case Filter.InPlace:
                    {
                        return GetAllInPlacePallets(searchTerm);
                    }
                case Filter.OnFloor:
                    {
                        return GetAllFloorPallets(searchTerm);
                    }
                case Filter.Missing:
                    {
                        return GetAllMissingPallets(searchTerm);
                    }
                default:
                    {
                        return null;
                    }
            }
        }

        private IQueryable<Pallet> GetAllMisplacedPallets(string searchTerm = "")
        {
            // This method gets either all misplaced pallets or all misplaced pallets who's id contains a search term

            var dbQuery = from pallet in context.Pallets
                          join shelf in context.Shelves on pallet.Id equals shelf.PalletId
                          where pallet.Id.Substring(2) != shelf.Id.Substring(2)
                          && ((searchTerm != null && !string.IsNullOrEmpty(searchTerm)) ? (pallet.Id.ToString().Contains(searchTerm)) : true)
                          select pallet;

            return dbQuery;
        }

        private IQueryable<Pallet> GetAllInPlacePallets(string searchTerm = "")
        {
            // This method gets either all In Place pallets or all misplaced pallets who's id contains a search term

            var dbQuery = from pallet in context.Pallets
                          join shelf in context.Shelves on pallet.Id equals shelf.PalletId
                          where pallet.Id.Substring(2) == shelf.Id.Substring(2)
                          && ((searchTerm != null && !string.IsNullOrEmpty(searchTerm)) ? (pallet.Id.ToString().Contains(searchTerm)) : true)
                          select pallet;

            return dbQuery;
        }

        private IQueryable<Pallet> GetAllFloorPallets(string searchTerm = "")
        {
            var dbQuery = from pallet in context.Pallets
                          where pallet.State == PalletState.Floor
                          && ((searchTerm != null && !string.IsNullOrEmpty(searchTerm)) ? (pallet.Id.ToString().Contains(searchTerm)) : true)
                          select pallet;

            return dbQuery;
        }

        private IQueryable<Pallet> GetAllMissingPallets(string searchTerm = "")
        {
            var dbQuery = from pallet in context.Pallets
                          where pallet.State == PalletState.Missing
                          && ((searchTerm != null && !string.IsNullOrEmpty(searchTerm)) ? (pallet.Id.ToString().Contains(searchTerm)) : true)
                          select pallet;

            return dbQuery;
        }


        public async Task<object> SearchPalletStatusesAsync(string searchTerm)
        {
            context = util.RemakeContext(context);

            var dbQuery = context.Pallets
                .Where(pallet => pallet.Id.ToString().Contains(searchTerm))
                .ToListAsync();

            var palletStatuses = await dbQuery;
            var result = await CreatePalletStatusResult(palletStatuses);
            return result;
        }

        public async Task<object> SearchPalletStatusesAsync(string searchTerm, Filter? filterTerm)
        {
            context = util.RemakeContext(context);
            var dbQuery = SelectAppropriateQuery(filterTerm, searchTerm);

            var palletStatuses = await dbQuery.ToListAsync();
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

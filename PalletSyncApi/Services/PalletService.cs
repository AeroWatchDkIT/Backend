using Microsoft.EntityFrameworkCore;
using PalletSyncApi.Classes;
using PalletSyncApi.Context;
using System.Threading.Tasks;

namespace PalletSyncApi.Services
{
    public class PalletService : IPalletService
    {
        PalletSyncDbContext context = new PalletSyncDbContext();

        GeneralUtilities util = new GeneralUtilities();
        public async Task<object> GetAllPalletsAsync()
        {
            context = util.RemakeContext(context);
            var pallets = await context.Pallets.ToListAsync();
            return WrapPalletList(pallets);
        }

        public async Task<object> GetPalletById(string id) {
            return await context.Pallets.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task AddPalletAsync(Pallet pallet)
        {
            context = util.RemakeContext(context);
            context.Pallets.Add(pallet);
            await context.SaveChangesAsync();
        }

        public async Task UpdatePalletAsync(Pallet pallet, bool updateLoc = true) {
            context = util.RemakeContext(context);
            var dbPallet = await context.Pallets.FirstOrDefaultAsync(e => e.Id == pallet.Id);

            if (dbPallet != null)
            {
                dbPallet.State = pallet.State;

                if (updateLoc)
                    dbPallet.Location = pallet.Location;
            }

            await context.SaveChangesAsync();
        }

        public async Task DeletePalletAsync(string palletId)
        {
            context = util.RemakeContext(context);
            var palletToRemove = await context.Pallets.FirstOrDefaultAsync(p => p.Id == palletId);
            var containerShelf = await context.Shelves.FirstOrDefaultAsync(s => s.PalletId == palletId);

            if (containerShelf != null)
            {
                containerShelf.PalletId = null;
                await context.SaveChangesAsync();
            }
            if (palletToRemove != null) {
                context.Pallets.Remove(palletToRemove);
                await context.SaveChangesAsync();
            }
        }
        private object WrapPalletList(object pallets)
        {
            var palletWrapper = new
            {
                pallets
            };
            return palletWrapper;
        }
    }
}

using Microsoft.EntityFrameworkCore;
using PalletSyncApi.Classes;
using PalletSyncApi.Context;

namespace PalletSyncApi.Services
{
    public class PalletService : IPalletService
    {
        PalletSyncDbContext context = new PalletSyncDbContext();
        GeneralUtilities util = new GeneralUtilities();
        public async Task<List<Pallet>> GetAllPalletsAsync()
        {
            context = util.RemakeContext(context);
            var pallets = await context.Pallets.ToListAsync();
            return pallets;
        }

        public async Task AddPalletAsync(Pallet pallet)
        {
            context = util.RemakeContext(context);
            context.Pallets.Add(pallet);
            await context.SaveChangesAsync();
        }

        public async Task UpdatePalletAsync(Pallet pallet) {
            context = util.RemakeContext(context);
            var dbPallet = await context.Pallets.FirstOrDefaultAsync(e => e.Id == pallet.Id);

            if (dbPallet != null)
            {
                dbPallet.State = pallet.State;
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
                context.Remove(palletToRemove);
                await context.SaveChangesAsync();
            }
           
        }
    }
}

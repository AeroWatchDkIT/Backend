using Microsoft.EntityFrameworkCore;
using PalletSyncApi.Classes;
using PalletSyncApi.Context;

namespace PalletSyncApi.Services
{
    public class PalletService : IPalletService
    {
        PalletSyncDbContext context = new PalletSyncDbContext();

        public async Task<object> GetAllPalletsAsync()
        {
            var pallets = await context.Pallets.ToListAsync();
            return WrapPalletList(pallets);
        }

        public async Task AddPalletAsync(Pallet pallet)
        {
            context.Pallets.Add(pallet);
            await context.SaveChangesAsync();
        }

        public async Task UpdatePalletAsync(Pallet pallet) { 
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

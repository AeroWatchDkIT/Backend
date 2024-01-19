using Microsoft.EntityFrameworkCore;
using PalletSyncApi.Classes;
using PalletSyncApi.Context;

namespace PalletSyncApi.Services
{
    public class ShelfService : IShelfService
    {
        PalletSyncDbContext context = new PalletSyncDbContext();

        public async Task<List<Shelf>> GetAllShelvesAsync()
        {
            var shelves = await context.Shelves.ToListAsync();
            return shelves;
        }

        public async Task AddShelfAsync(Shelf shelf)
        {
            context.Shelves.Add(shelf);
            await context.SaveChangesAsync();
        }

        public async Task UpdateShelfAsync(Shelf shelf)
        {
            var dbShelf = await context.Shelves.FirstOrDefaultAsync(e => e.Id == shelf.Id);

            if (dbShelf != null)
            {
                dbShelf.PalletId = shelf.PalletId;
                dbShelf.Location = shelf.Location;
            }

            await context.SaveChangesAsync();
        }

        public async Task DeleteShelfAsync(string shelfId)
        {
            var shelfToRemove = await context.Shelves.FirstOrDefaultAsync(p => p.Id == shelfId);

            if (shelfToRemove != null)
            {
                context.Shelves.Remove(shelfToRemove);
                await context.SaveChangesAsync();
            }
        }
    }
}

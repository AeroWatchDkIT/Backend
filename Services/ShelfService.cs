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
            throw new NotImplementedException();
        }

        public async Task UpdateShelfAsync(Shelf shelf)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteShelfAsync(string shelfId)
        {
            throw new NotImplementedException();
        }
    }
}

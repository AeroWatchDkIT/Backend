using PalletSyncApi.Classes;

namespace PalletSyncApi.Services
{
    public interface IShelfService
    {
        public Task<List<Shelf>> GetAllShelvesAsync();
        public Task AddShelfAsync(Shelf shelf);
        public Task UpdateShelfAsync(Shelf shelf);
        public Task DeleteShelfAsync(string shelfId);
    }
}

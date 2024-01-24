using PalletSyncApi.Classes;

namespace PalletSyncApi.Services
{
    public interface IShelfService
    {
        public Task<object> GetAllShelvesAsync();
        public Task AddShelfAsync(Shelf shelf);
        public Task UpdateShelfHardwareAsync(Shelf shelf);
        public Task UpdateShelfFrontendAsync(Shelf shelf, bool updateLoc);
        public Task DeleteShelfAsync(string shelfId);
    }
}

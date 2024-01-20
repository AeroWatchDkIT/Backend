using PalletSyncApi.Classes;

namespace PalletSyncApi.Services
{
    public interface IPalletService
    {
        public Task<object> GetAllPalletsAsync();
        public Task AddPalletAsync(Pallet pallet);
        public Task UpdatePalletAsync(Pallet pallet);
        public Task DeletePalletAsync(string palletId);
    }
}

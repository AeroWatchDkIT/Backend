using PalletSyncApi.Classes;

namespace PalletSyncApi.Services
{
    public interface IPalletService
    {
        public Task<List<Pallet>> GetAllPalletsAsync();
        public Task AddPalletAsync(Pallet pallet);
        public Task UpdatePalletAsync(Pallet pallet);
        public Task DeletePalletAsync(string palletId);
    }
}

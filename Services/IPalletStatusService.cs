using PalletSyncApi.Classes;

namespace PalletSyncApi.Services
{
    public interface IPalletStatusService
    {
        public Task<object> GetAllPalletStatusesAsync();
        public Task<object> SearchPalletStatusesAsync(UniversalSearchTerm query);
    }
}

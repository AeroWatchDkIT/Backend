using PalletSyncApi.Classes;
using PalletSyncApi.Enums;

namespace PalletSyncApi.Services
{
    public interface IPalletStatusService
    {
        public Task<object> GetAllPalletStatusesAsync(Filter? filterTerm);
        public Task<object> GetAllPalletStatusesAsync();
        public Task<object> SearchPalletStatusesAsync(string searchTerm, Filter? filterTerm);
        public Task<object> SearchPalletStatusesAsync(string searchTerm);
    }
}

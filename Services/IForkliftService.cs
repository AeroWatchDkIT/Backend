using PalletSyncApi.Classes;

namespace PalletSyncApi.Services
{
    public interface IForkliftService
    {
        public Task<object> GetAllForkliftsAsync();
        public Task AddForkliftAsync(Forklift forklift);
        public Task<object> SearchForkliftsAsync(SearchForklift query);
        public Task<object> GetForkliftById(string id);
    }
}

using PalletSyncApi.Classes;

namespace PalletSyncApi.Services
{
    public interface IForkliftService
    {
        public Task<List<Forklift>> GetAllForkliftsAsync();
        public Task AddForkliftAsync(Forklift forklift);
        public Task<List<ForkliftUiResult>> SearchForkliftsAsync(string requestedId);
    }
}

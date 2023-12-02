using PalletSyncApi.Classes;

namespace PalletSyncApi.Services
{
    public interface IForkliftService
    {
        public Task<List<Forklift>> GetAllForkliftsAsync();
        public Task AddForkliftAsync(Forklift forklift);
    }
}

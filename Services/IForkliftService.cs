using PalletSyncApi.Classes;

namespace PalletSyncApi.Services
{
    public interface IForkliftService
    {
        public Task<object> GetAllForkliftsAsync();
        public Task AddForkliftAsync(Forklift forklift);
        public Task<object> SearchForkliftsAsync(UniversalSearchTerm query);
        public Task<object> GetForkliftByIdAsync(string id);
        public Task<bool> DeleteObjectByIdAsync(string id);
        public Task<bool> UpdateForkliftByIdAsync(string newId, string oldId);
    }
}

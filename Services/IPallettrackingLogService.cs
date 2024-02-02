using PalletSyncApi.Classes;

namespace PalletSyncApi.Services
{
    public interface IPalletTrackingLogService
    {
        public Task AddPalletTrackingLogAsync(CompareTwoCodesJson data);
        public Task<object> GetAllTrackingLogsAsync();
    }
}

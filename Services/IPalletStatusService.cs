namespace PalletSyncApi.Services
{
    public interface IPalletStatusService
    {
        public Task<object> GetAllPalletStatusesAsync();
    }
}

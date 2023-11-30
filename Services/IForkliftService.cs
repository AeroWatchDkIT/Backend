using PalletSyncApi.Classes;

namespace PalletSyncApi.Services
{
    public interface IForkliftService
    {
        public List<Forklift> GetAllForklifts();
    }
}

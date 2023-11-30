using PalletSyncApi.Classes;
using PalletSyncApi.Context;



namespace PalletSyncApi.Services
{

    

public class ForkliftService: IForkliftService
    {
        PalletSyncDbContext context = new PalletSyncDbContext();
        
        public List<Forklift> GetAllForklifts()
        {
            var Forklifts = context.Forklifts.ToList();
            return Forklifts;
        }

    }
}

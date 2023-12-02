using Microsoft.EntityFrameworkCore;
using PalletSyncApi.Classes;
using PalletSyncApi.Context;



namespace PalletSyncApi.Services
{

public class ForkliftService: IForkliftService
    {
        PalletSyncDbContext context = new PalletSyncDbContext();
        
        public async Task<List<Forklift>> GetAllForkliftsAsync()
        {
            var Forklifts = await context.Forklifts.ToListAsync();
            return Forklifts;
        }

        public async Task AddForkliftAsync(Forklift forklift)
        {
            context.Forklifts.Add(forklift);
            await context.SaveChangesAsync();
        }



    }
}

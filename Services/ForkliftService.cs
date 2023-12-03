using Microsoft.EntityFrameworkCore;
using PalletSyncApi.Classes;
using PalletSyncApi.Context;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;



namespace PalletSyncApi.Services
{

public class ForkliftService: IForkliftService
    {
        PalletSyncDbContext context = new PalletSyncDbContext();
        
        public async Task<List<Forklift>> GetAllForkliftsAsync()
        {
            var Forklifts = await context.Forklifts.ToListAsync();
            return Forklifts;

            // This one will need to be reworked a bit to return the same objects as SearchForkliftsAsync
        }

        public async Task AddForkliftAsync(Forklift forklift)
        {
            context.Forklifts.Add(forklift);
            await context.SaveChangesAsync();
        }

        public async Task<List<ForkliftUiResult>> SearchForkliftsAsync(string requestedId)
        {

            var query = from forklift in context.Forklifts
                         join user in context.Users on forklift.LastUserId equals user.Id into ps
                         from user in ps.DefaultIfEmpty()
                         where forklift.Id.Contains(requestedId) || user.FirstName.Contains(requestedId) || user.LastName.Contains(requestedId)
                         select new
                         {
                             id = forklift.Id,
                             lastUser = $"{user.FirstName} {user.LastName}",
                             lastPallet = forklift.LastPalletId,
                         };

            var result = await query.ToListAsync();

            List<ForkliftUiResult> readyResult = new List<ForkliftUiResult>();

            foreach(var x in result)
            {
                var additionalResult = new ForkliftUiResult(x.id, x.lastUser, x.lastPallet);
                readyResult.Add(additionalResult);
            }
            
            //look over this one, might be a cleaner way to do it, test it well

            return readyResult;
        }


    }
}

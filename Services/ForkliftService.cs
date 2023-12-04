using Microsoft.EntityFrameworkCore;
using PalletSyncApi.Classes;
using PalletSyncApi.Context;
using System.IO.Pipelines;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;



namespace PalletSyncApi.Services
{

public class ForkliftService: IForkliftService
    {
        PalletSyncDbContext context = new PalletSyncDbContext();
        
        public async Task<object> GetAllForkliftsAsync()
        {
            var dbQuery = from forklift in context.Forklifts
                        join user in context.Users on forklift.LastUserId equals user.Id into ps
                        from user in ps.DefaultIfEmpty()
                        select new
                        {
                            id = forklift.Id,
                            lastUser = $"{user.FirstName} {user.LastName}",
                            lastPallet = forklift.LastPalletId,
                        };

            return await dbQuery.ToListAsync();
        }

        public async Task AddForkliftAsync(Forklift forklift)
        {
            context.Forklifts.Add(forklift);
            await context.SaveChangesAsync();
        }

        public async Task<object> SearchForkliftsAsync(SearchForklift query)
        {

            var dbQuery = from forklift in context.Forklifts
                         join user in context.Users on forklift.LastUserId equals user.Id into ps
                         from user in ps.DefaultIfEmpty()
                         where forklift.Id.Contains(query.SearchTerm) || user.FirstName.Contains(query.SearchTerm) || user.LastName.Contains(query.SearchTerm)
                         select new
                         {
                             id = forklift.Id,
                             lastUser = $"{user.FirstName} {user.LastName}",
                             lastPallet = forklift.LastPalletId,
                         };

            return await dbQuery.ToListAsync();
            // Hey check it out Kacper, no need for making a list of objects with some new class, this works just fine!
        }

        public async Task<object> GetForkliftById(string id)
        {

            var dbQuery = from forklift in context.Forklifts
                          join user in context.Users on forklift.LastUserId equals user.Id into ps
                          from user in ps.DefaultIfEmpty()
                          where forklift.Id.Equals(id)
                          select new
                          {
                              id = forklift.Id,
                              lastUser = $"{user.FirstName} {user.LastName}",
                              lastPallet = forklift.LastPalletId,
                          };

            return await dbQuery.FirstOrDefaultAsync();
        }

    }
}

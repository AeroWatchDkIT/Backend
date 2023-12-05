using Microsoft.EntityFrameworkCore;
using PalletSyncApi.Classes;
using PalletSyncApi.Context;



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
                            lastUserId = user.Id,
                            lastUser = user != null? $"{user.FirstName} {user.LastName}" : null,
                            lastPallet = forklift.LastPalletId
                        };

            var forklifts = await dbQuery.ToListAsync();
            return WrapListOfForklifts(forklifts);

            // the reason for wrapping is https://youtu.be/60F8rzP5nQo?si=istwlDOjK0S2XtJO&t=295
        }

        public async Task AddForkliftAsync(Forklift forklift)
        {
            try
            {
                context.Forklifts.Add(forklift);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");

                /*Encountered a problem while testing where if I purposefully try to add a forklift with an id that already exists, 
                 an InvalidOperationException or DbUpdateException would get thrown, but after catching and handling this exception, I
                could not add new forklifts with Ids that dont exist in the database, because for some reason the context was still
                trying to add the forklift with the id that originally causes the exception. A fix that worked for me was disposing of the context and 
                making it anew, another possible solution which may be explored is querying the db to see if an id already exists prior to insertion*/

                context.Dispose();
                context = new PalletSyncDbContext();

                // This throws the exception up the call stack so that it can be caught again in the forkliftController class, and return an appropriate
                // message back to the person making the request
                throw ex;
            }
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
                             lastUserId = user.Id,
                             lastUser = user != null ? $"{user.FirstName} {user.LastName}" : null,
                             lastPallet = forklift.LastPalletId
                         };

            var forklifts = await dbQuery.ToListAsync();
            return WrapListOfForklifts(forklifts);
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
                              lastUserId = user.Id,
                              lastUser = user != null ? $"{user.FirstName} {user.LastName}" : null,
                              lastPallet = forklift.LastPalletId
                          };

            var result = await dbQuery.FirstOrDefaultAsync();
            return result;
        }

        public async Task<bool> DeleteObjectById(string id)
        {
            var forkliftToDelete = await context.Forklifts.FindAsync(id);

            if (forkliftToDelete != null)
            {
                context.Forklifts.Remove(forkliftToDelete);
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        private object WrapListOfForklifts(object forklifts)
        {
            var forkliftWrapper = new
            {
                forklifts
            };
            return forkliftWrapper;
        }

    }
}

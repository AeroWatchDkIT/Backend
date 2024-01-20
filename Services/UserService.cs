using Microsoft.EntityFrameworkCore;
using PalletSyncApi.Classes;
using PalletSyncApi.Context;

namespace PalletSyncApi.Services
{
    public class UserService : IUserService
    {
        PalletSyncDbContext context = new PalletSyncDbContext();

        public async Task<object> GetAllUsersAsync()
        {
            var users = await context.Users.Select(u => new {u.Id, u.UserType, u.FirstName, u.LastName, u.ForkliftCertified, u.IncorrectPalletPlacements}).ToListAsync();
            return Wrap(users);

            // the reason for wrapping is https://youtu.be/60F8rzP5nQo?si=istwlDOjK0S2XtJO&t=295
        }

        public async Task AddUserAsync(User user)
        {
            try
            {
                context.Users.Add(user);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                context.Dispose();
                context = new PalletSyncDbContext();
                throw;
            }
        }

        public async Task<object> GetUserByIdAsync(string id)
        {
            var user = await context.Users
                .Where(u => u.Id == id)
                .Select(u => new { u.Id, u.UserType, u.FirstName, u.LastName, u.ForkliftCertified, u.IncorrectPalletPlacements })
                .FirstOrDefaultAsync();

            return user;
        }

        public async Task<bool> DeleteUserByIdAsync(string id)
        {
            var userToDelete = await context.Users.FindAsync(id);
            var relatedForklifts = await context.Forklifts.Where(f => f.LastUserId == id).ToListAsync();

            if (userToDelete != null)
            {
                if (relatedForklifts.Count > 0)
                {
                    foreach (var forklift in  relatedForklifts)
                    {
                        forklift.LastUserId = null;
                    }
                }

                context.Users.Remove(userToDelete);
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        private object Wrap(object users)
        {
            var wrapper = new
            {
                users
            };
            return wrapper;
        }
    }
}

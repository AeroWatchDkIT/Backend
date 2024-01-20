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

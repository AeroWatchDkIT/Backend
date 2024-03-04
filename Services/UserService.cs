using Microsoft.EntityFrameworkCore;
using PalletSyncApi.Classes;
using PalletSyncApi.Context;
using PalletSyncApi.Enums;
using System.Net.WebSockets;
using System.Security.Authentication;

namespace PalletSyncApi.Services
{
    public class UserService : IUserService
    {
        PalletSyncDbContext context = new PalletSyncDbContext();

        struct UserAsString
        {
            public string Id { get; set; }
            public string UserType { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Passcode { get; set; }
            public string ForkliftCertified { get; set; }
            public string IncorrectPalletPlacements { get; set; }
        }

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

        public async Task<bool> UpdateUserAsync(User user)
        {
            var userToUpdate = await context.Users.FindAsync(user.Id);

            if (userToUpdate != null)
            {
                try
                {
                    userToUpdate.Id = user.Id;
                    userToUpdate.UserType = user.UserType;
                    userToUpdate.FirstName = user.FirstName;
                    userToUpdate.LastName = user.LastName;
                    userToUpdate.Passcode = user.Passcode;
                    userToUpdate.ForkliftCertified = user.ForkliftCertified;
                    userToUpdate.IncorrectPalletPlacements = user.IncorrectPalletPlacements;

                    await context.SaveChangesAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");

                    context.Dispose();
                    context = new PalletSyncDbContext();
                    throw;
                }
            }
            return false;
        }

        public async Task<bool> AuthenticateUserAsync(string userId, string passCode, bool requestFromAdmin)
        {
            var user = await context.Users.FindAsync(userId);

            if (user != null && passCode == user.Passcode)
            {
                if (requestFromAdmin && user.UserType != UserType.Admin)
                {
                    throw new UnauthorizedAccessException();
                }

                return true;
            }

            throw new InvalidCredentialException();
        }
        public async Task<object> SearchUsersAsync(UniversalSearchTerm query)
        {
            
            var allUsers = await context.Users.ToListAsync();
            var allUsersAsString = new List<UserAsString>();

            foreach (var user in allUsers)
            {
                var stringUser = new UserAsString { 
                    Id = user.Id.ToString().ToLower(), 
                    UserType = user.UserType.ToString().ToLower(),
                    FirstName = user.FirstName.ToString().ToLower(), 
                    LastName = user.LastName.ToString().ToLower(),
                    Passcode = user.Passcode.ToString().ToLower(),
                    ForkliftCertified = user.ForkliftCertified.ToString().ToLower(),
                    IncorrectPalletPlacements = user.IncorrectPalletPlacements.ToString().ToLower()
                };

                allUsersAsString.Add(stringUser);
            }

            // I realize how convoluted and inefficient the code in this function is, but for some reason LINQ is not able to translate queries into SQL queries
            // if any of the fields have .ToString() being called in the LINQ query itself. So in the 'Where' clause, if I wrote 'u.UserType.ToString()' it would
            // throw an exception. Creating a user struct where all the fields are converted to string BEFORE the LINQ query was the only working solution I could come up with.
            // For the final release we should come up with a better way of doing this when we're just cleaning up code, but this works for now. If you have any suggestions
            // for a better implementation, let me know - Kacper
            var users = allUsersAsString
                 .Where(u => new[]
                    {
                        u.Id,
                        u.UserType,
                        u.FirstName,
                        u.LastName,
                        u.ForkliftCertified,
                        u.IncorrectPalletPlacements
                    }.Any(u => u.Contains(query.SearchTerm.ToLower())))
                .Select(u => new { u.Id, u.UserType, u.FirstName, u.LastName, ForkliftCertified = Convert.ToBoolean(u.ForkliftCertified), IncorrectPalletPlacements = Convert.ToInt32(u.IncorrectPalletPlacements) }).ToList();

            return Wrap(users);
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

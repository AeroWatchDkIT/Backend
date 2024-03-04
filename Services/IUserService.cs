using PalletSyncApi.Classes;
using System.Threading.Tasks;

namespace PalletSyncApi.Services
{
    public interface IUserService
    {
        public Task<object> GetAllUsersAsync();
        public Task AddUserAsync(User user);
        public Task<object> GetUserByIdAsync(string id);
        public Task<bool> DeleteUserByIdAsync(string id);
        public Task<bool> UpdateUserAsync(User user);
        public Task<bool> UpdateUserPasswordAsync(string userId, string newPassword);
        public Task<bool> AuthenticateUserAsync(string userId, string passCode, bool requestFromAdmin);
        public Task<object> SearchUsersAsync(UniversalSearchTerm query);
    }
}

using PalletSyncApi.Classes;

namespace PalletSyncApi.Services
{
    public interface IUserService
    {
        public Task<object> GetAllUsersAsync();
        public Task AddUserAsync(Forklift forklift);
        //public Task<object> SearchUsersAsync(SearchUser query);
        public Task<object> GetUserByIdAsync(string id);
        public Task<bool> DeleteUserByIdAsync(string id);
        public Task<bool> UpdateUser(string newId, string oldId);
    }
}

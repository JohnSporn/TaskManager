using TaskManager.Data.Models;

namespace TaskManager.Data.Repositories
{
    public interface IUserRepository
    {
        public Task<int> User_Insert(string? name);
        public IAsyncEnumerable<User> Users_Get();
        public Task<User?> Users_GetByName(string? name);
    }
}

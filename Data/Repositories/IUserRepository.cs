using TaskManager.Data.Models;

namespace TaskManager.Data.Repositories
{
    public interface IUserRepository
    {
        public Task<int> User_Insert(User user);
        public IAsyncEnumerable<User> Users_Get();
        public Task<User?> Users_GetById(string? id);
    }
}

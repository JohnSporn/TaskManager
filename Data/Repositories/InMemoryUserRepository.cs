using TaskManager.Data.Models;

namespace TaskManager.Data.Repositories
{
    public class InMemoryUserRepository : IUserRepository
    {
        private static readonly List<User> _users = new()
        {
            new User
            {
                Id = 1,
                Name = "Test User",
                User_Id = "test-user-id",
                Tasks = new List<TaskItem>()
            }
        };

        public async Task<User?> Users_GetById(string? id)
        {
            await Task.Delay(1); // Simulate async operation
            return _users.FirstOrDefault(u => u.User_Id == id);
        }

        public async Task<int> User_Insert(User user)
        {
            await Task.Delay(1); // Simulate async operation
            user.Id = _users.Any() ? _users.Max(u => u.Id) + 1 : 1;
            user.Tasks = new List<TaskItem>();
            _users.Add(user);
            return user.Id;
        }

        public async IAsyncEnumerable<User> Users_Get()
        {
            await Task.Delay(1); // Simulate async operation
            foreach (var user in _users)
            {
                yield return user;
            }
        }
    }
}
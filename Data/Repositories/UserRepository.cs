using Dapper;
using Microsoft.Data.SqlClient;
using TaskManager.Data.Models;

namespace TaskManager.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;

        public UserRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("TaskManager");
        }

        public IAsyncEnumerable<User> Users_Get()
        {
            throw new NotImplementedException();
        }

        public async Task<User?> Users_GetById(string? id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = @"SELECT Id, [Name], User_Id FROM [User] WHERE User_Id = @Id";
                return await connection.QuerySingleOrDefaultAsync<User>(sql, new { Id = id });
            }
        }

        public async Task<int> User_Insert(User user)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = @"INSERT INTO [User] ([Name], User_Id)
                            VALUES (@Name, @UserId)";
                return await connection.ExecuteAsync(sql, new { Name = user.Name, UserId = user.User_Id });
            }
        }
    }
}

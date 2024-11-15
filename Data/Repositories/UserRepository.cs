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

        public async Task<User?> Users_GetByName(string? name)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = @"SELECT Id, [Name] FROM [User] WHERE [Name] = @Name";
                return await connection.QuerySingleOrDefaultAsync<User>(sql, new { Name = name });
            }
        }

        public async Task<int> User_Insert(string name)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = @"INSERT INTO [User] ([Name])
                            VALUES (@Name)";
                return await connection.ExecuteAsync(sql, new { Name = name });
            }
        }
    }
}

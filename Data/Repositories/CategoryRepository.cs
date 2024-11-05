using Dapper;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using TaskManager.Data.Models;

namespace TaskManager.Data.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly string _connectionString;

        public CategoryRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("TaskManager");
        }

        public async IAsyncEnumerable<Category> Categories_Get()
        {
            var connection = new SqlConnection(_connectionString);
            var sql = "SELECT * FROM Category";
            var categories = await connection.QueryAsync<Category>(sql);
            foreach (var item in categories)
            {
                yield return item;
            }
        }

        public async Task<Category> Categories_GetById(int? id)
        {
            var connection = new SqlConnection(_connectionString);
            var sql = "SELECT * FROM Category WHERE Id = @Id";
            return await connection.QuerySingleAsync<Category>(sql, new { Id = id });
        }

        public async Task<int> Category_Delete(Category category)
        {
            var connection = new SqlConnection(_connectionString);
            var sql1 = "DELETE FROM Category WHERE Id = @Id";
            return await connection.ExecuteAsync(sql1, new { Id = category.Id });
        }

        public async Task<int> Category_Upsert(Category category)
        {
            var connection = new SqlConnection(_connectionString);
            if (category.Id == 0)
            {
                var sql1 = "INSERT INTO Category (Name) " +
                           "VALUES (@Name)";
                return await connection.ExecuteAsync(sql1, new { Name = category.Name });
            }
            else
            {
                var sql2 = "UPDATE Category SET Name = @Name WHERE Id = @Id";
                return await connection.ExecuteAsync(sql2, new { Id = category.Id, Name = category.Name });
            }
        }
    }
}

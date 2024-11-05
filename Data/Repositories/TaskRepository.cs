

using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using System.Threading.Tasks;
using TaskManager.Data.Models;

namespace TaskManager.Data.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly string _connectionString;

        public TaskRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("TaskManager");
        }

        public async IAsyncEnumerable<TaskItem> Tasks_Get()
        {
            var connection = new SqlConnection(_connectionString);
            var sql = "SELECT * FROM TaskItem";
            var tasks = await connection.QueryAsync<TaskItem>(sql);
            foreach (var item in tasks)
            {
                yield return item;
            }
        }

        public async Task<TaskItem> Tasks_GetById(int id)
        {
            var connection = new SqlConnection(_connectionString);
            var sql = "SELECT * FROM TaskItem WHERE Id = @Id";
            return await connection.QuerySingleAsync<TaskItem>(sql, new { Id = id });
        }

        public async Task<int> Task_Delete(TaskItem Task)
        {
            var connection = new SqlConnection(_connectionString);
            var sql1 = "DELETE FROM TaskManager WHERE Id = @Id";
            return await connection.ExecuteAsync(sql1, new { Id = Task.Id });
        }

        public async Task<int> Task_Upsert(TaskItem task)
        {
            var connection = new SqlConnection(_connectionString);
            if (task.Id == 0)
            {
                var sql1 = "INSERT INTO TaskManager (Name, DueDate, Priority, Category, IsComplete) " +
                           "VALUES (@Name, @DueDate, @Priority, @Category, @IsComplete)";
                return await connection.ExecuteAsync(sql1, new
                {
                    Name = task.Name,
                    DueDate = task.DueDate,
                    Priority = task.Priority,
                    Category = task.Category,
                    IsComplete = task.IsComplete
                });
            }
            else
            {
                var sql2 = "UPDATE TaskManager SET Name = @Name, DueDate = @DueDate, Priority = @Priority, Category = @Category, " +
                           "IsComplete = @IsComplete WHERE Id = @Id";
                return await connection.ExecuteAsync(sql2, new
                {
                    Id = task.Id,
                    Name = task.Name,
                    DueDate = task.DueDate,
                    Priority = task.Priority,
                    Category = task.Category,
                    IsComplete = task.IsComplete
                });
            }
        }
    }
}



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
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = @"SELECT T.Id, 
                                   T.Name, 
                                   T.DueDate, 
                                   T.Priority,
                                   T.CategoryId,
                                   C.Name, 
                                   T.IsComplete
                            FROM TaskItem T INNER JOIN Category C
                            ON T.CategoryId = C.Id";
                var tasks = await connection.QueryAsync<TaskItem, Category, TaskItem>(sql, (task, category) =>
                {
                    task.Category = category;
                    return task;
                },
                splitOn: "CategoryId");

                foreach (var item in tasks)
                {
                    yield return item;
                }
            }
        }

        public async Task<TaskItem> Tasks_GetById(int? id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = "SELECT T.Name, T.DueDate, T.Priority, C.Name, T.IsComplete " +
                    "FROM TaskItem T INNER JOIN Category C ON t.CategoryId = C.Id " +
                    "WHERE T.Id = @Id";
                return await connection.QuerySingleAsync<TaskItem>(sql, new { Id = id });
            }
        }

        public async Task<int> Task_Delete(TaskItem Task)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql1 = "DELETE FROM TaskItem WHERE Id = @Id";
                return await connection.ExecuteAsync(sql1, new { Id = Task.Id });
            }
        }

        public async Task<int> Task_Upsert(TaskItem task)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                if (task.Id == 0)
                {
                    var sql1 = "INSERT INTO TaskItem (Name, DueDate, Priority, CategoryId, IsComplete) " +
                               "VALUES (@Name, @DueDate, @Priority, @Category, @IsComplete)";
                    return await connection.ExecuteAsync(sql1, new
                    {
                        Name = task.Name,
                        DueDate = task.DueDate,
                        Priority = task.Priority,
                        Category = task.Category.Id,
                        IsComplete = task.IsComplete
                    });
                }
                else
                {
                    var sql2 = "UPDATE TaskItem SET Name = @Name, DueDate = @DueDate, Priority = @Priority, CategoryId = @Category, " +
                               "IsComplete = @IsComplete WHERE Id = @Id";
                    return await connection.ExecuteAsync(sql2, new
                    {
                        Id = task.Id,
                        Name = task.Name,
                        DueDate = task.DueDate,
                        Priority = task.Priority,
                        Category = task.Category.Id,
                        IsComplete = task.IsComplete
                    });
                }
            }
        }
    }
}

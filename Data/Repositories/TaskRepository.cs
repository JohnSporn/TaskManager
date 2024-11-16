

using Dapper;
using Microsoft.AspNetCore.SignalR;
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

        public async IAsyncEnumerable<TaskItem?> Tasks_Get(string userId)
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
                            ON T.CategoryId = C.Id
                            WHERE T.UserId = @UserId";
                var tasks = await connection.QueryAsync<TaskItem, Category, TaskItem>(sql, (task, category) =>
                {
                    task.Category = category;
                    return task;
                },
                new { UserId = userId },
                splitOn: "CategoryId");

                foreach (var item in tasks)
                {
                    yield return item;
                }
            }
        }

        public async Task<TaskItem> Tasks_GetById(int? id, string userId)
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
                            ON T.CategoryId = C.Id
                            WHERE T.Id = @Id AND T.UserId = @UserId";
                var tasks = await connection.QueryAsync<TaskItem, Category, TaskItem>(sql, (task, category) =>
                {
                    task.Category = category;
                    return task;
                },
                new { Id = id, UserId = userId },
                splitOn: "CategoryId");

                return tasks.FirstOrDefault();
            }
        }

        public async Task<int> Task_Delete(TaskItem Task)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql1 = "DELETE FROM TaskItem WHERE Id = @Id AND UserId = @UserId";
                return await connection.ExecuteAsync(sql1, new { Id = Task.Id, UserId = Task.UserId });
            }
        }

        public async Task<int> Task_Upsert(TaskItem task)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                if (task.Id == 0)
                {
                    var sqlInsert = @"INSERT INTO TaskItem (Name, DueDate, Priority, CategoryId, IsComplete, UserId)
                                 VALUES (@Name, @DueDate, @Priority, @Category, @IsComplete, @UserId)";
                    return await connection.ExecuteAsync(sqlInsert, new
                    {
                        Name = task.Name,
                        DueDate = task.DueDate,
                        Priority = task.Priority,
                        Category = task.Category.Id,
                        IsComplete = task.IsComplete,
                        UserId = task.UserId
                    });
                }
                else
                {
                    using (var transaction = await connection.BeginTransactionAsync())
                    {
                        try
                        {
                            connection.Open();
                            var sqlUpdateTask = @"UPDATE TaskItem 
                                          SET Name = @Name, 
                                              DueDate = @DueDate, 
                                              Priority = @Priority, 
                                              CategoryId = @Category,
                                              IsComplete = @IsComplete 
                                          WHERE Id = @Id AND UserId = @UserId";
                            var result = await connection.ExecuteAsync(sqlUpdateTask, new
                            {
                                Id = task.Id,
                                Name = task.Name,
                                DueDate = task.DueDate,
                                Priority = task.Priority,
                                Category = task.Category.Id,
                                IsComplete = task.IsComplete,
                                UserId = task.UserId
                            }, transaction: transaction);

                            await transaction.CommitAsync();

                            return result;
                        }
                        catch(Exception ex)
                        {
                            await transaction.RollbackAsync();
                            Console.WriteLine(ex.ToString());
                            return 0;
                        }
                    }
                }
            }
        }
    }
}

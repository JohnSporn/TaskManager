using TaskManager.Data.Models;

namespace TaskManager.Data.Repositories
{
    public class InMemoryTaskRepository : ITaskRepository
    {
        private static readonly List<TaskItem> _tasks = new()
        {
            new TaskItem
            {
                Id = 1,
                Name = "Complete project proposal",
                DueDate = DateTime.Now.AddDays(3),
                Priority = Priority.High,
                IsComplete = false,
                Category = new Category { Id = 1, Name = "Work" },
                UserId = "test-user-id",
                ReminderDate = DateTime.Now.AddDays(2),
                ReminderEnabled = true,
                AssignedToUserId = "test-user-id",
                AssignedToUserName = "Test User",
                Notes = "High priority project proposal due soon",
                CreatedDate = DateTime.Now.AddDays(-5)
            },
            new TaskItem
            {
                Id = 2,
                Name = "Review code changes",
                DueDate = DateTime.Now.AddDays(1),
                Priority = Priority.Medium,
                IsComplete = true,
                Category = new Category { Id = 2, Name = "Development" },
                UserId = "test-user-id",
                CompletedDate = DateTime.Now.AddHours(-2),
                AssignedToUserId = "test-user-id",
                AssignedToUserName = "Test User",
                Notes = "Code review completed successfully",
                CreatedDate = DateTime.Now.AddDays(-3)
            },
            new TaskItem
            {
                Id = 3,
                Name = "Update documentation",
                DueDate = DateTime.Now.AddDays(5),
                Priority = Priority.Low,
                IsComplete = false,
                Category = new Category { Id = 1, Name = "Work" },
                UserId = "test-user-id",
                ReminderDate = DateTime.Now.AddDays(4),
                ReminderEnabled = true,
                SharedWithUserIds = new List<string> { "teammate-1", "teammate-2" },
                Notes = "Documentation needs to be updated for new features",
                CreatedDate = DateTime.Now.AddDays(-2)
            },
            new TaskItem
            {
                Id = 4,
                Name = "Plan team meeting",
                DueDate = DateTime.Now.AddDays(2),
                Priority = Priority.Medium,
                IsComplete = false,
                Category = new Category { Id = 3, Name = "Management" },
                UserId = "test-user-id",
                ReminderDate = DateTime.Now.AddDays(1),
                ReminderEnabled = true,
                AssignedToUserId = "manager-1",
                AssignedToUserName = "Project Manager",
                SharedWithUserIds = new List<string> { "team-member-1", "team-member-2" },
                Notes = "Quarterly planning meeting with the team",
                CreatedDate = DateTime.Now.AddDays(-1)
            }
        };

        public async IAsyncEnumerable<TaskItem?> Tasks_Get(string userId)
        {
            await Task.Delay(1); // Simulate async operation
            foreach (var task in _tasks.Where(t => t.UserId == userId))
            {
                yield return task;
            }
        }

        public async Task<TaskItem> Tasks_GetById(int? id, string userId)
        {
            await Task.Delay(1); // Simulate async operation
            return _tasks.FirstOrDefault(t => t.Id == id && t.UserId == userId) ?? new TaskItem 
            { 
                Id = 0, 
                Name = "", 
                UserId = userId ?? "", 
                Category = new Category { Id = 0, Name = "" } 
            };
        }

        public async Task<int> Task_Upsert(TaskItem task)
        {
            await Task.Delay(1); // Simulate async operation
            var existingTask = _tasks.FirstOrDefault(t => t.Id == task.Id && t.UserId == task.UserId);
            if (existingTask != null)
            {
                existingTask.Name = task.Name;
                existingTask.DueDate = task.DueDate;
                existingTask.Priority = task.Priority;
                existingTask.IsComplete = task.IsComplete;
                existingTask.Category = task.Category;
                return 1;
            }
            else
            {
                task.Id = _tasks.Any() ? _tasks.Max(t => t.Id) + 1 : 1;
                _tasks.Add(task);
                return task.Id;
            }
        }

        public async Task<int> Task_Delete(TaskItem task)
        {
            await Task.Delay(1); // Simulate async operation
            var existingTask = _tasks.FirstOrDefault(t => t.Id == task.Id && t.UserId == task.UserId);
            if (existingTask != null)
            {
                _tasks.Remove(existingTask);
                return 1;
            }
            return 0;
        }
    }
}
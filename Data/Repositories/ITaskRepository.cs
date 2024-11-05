using TaskManager.Data.Models;

namespace TaskManager.Data.Repositories
{
    public interface ITaskRepository
    {
        public Task<int> Task_Upsert(TaskItem task);
        public Task<int> Task_Delete(TaskItem task);
        public IAsyncEnumerable<TaskItem> Tasks_Get();
        public Task<TaskItem> Tasks_GetById(int id);
    }
}

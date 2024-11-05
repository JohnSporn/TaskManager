using TaskManager.Data.Models;

namespace TaskManager.Data.Repositories
{
    public interface ICategoryRepository
    {
        public Task<int> Category_Upsert(Category category);
        public Task<int> Category_Delete(Category category);
        public IAsyncEnumerable<Category> Categories_Get();
        public Task<Category> Categories_GetById(int? id);
    }
}

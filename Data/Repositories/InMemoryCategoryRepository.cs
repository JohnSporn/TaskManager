using TaskManager.Data.Models;

namespace TaskManager.Data.Repositories
{
    public class InMemoryCategoryRepository : ICategoryRepository
    {
        private static readonly List<Category> _categories = new()
        {
            new Category { Id = 1, Name = "Work" },
            new Category { Id = 2, Name = "Development" },
            new Category { Id = 3, Name = "Management" },
            new Category { Id = 4, Name = "Personal" }
        };

        public async IAsyncEnumerable<Category> Categories_Get()
        {
            await Task.Delay(1); // Simulate async operation
            foreach (var category in _categories)
            {
                yield return category;
            }
        }

        public async Task<Category> Categories_GetById(int? id)
        {
            await Task.Delay(1); // Simulate async operation
            return _categories.FirstOrDefault(c => c.Id == id) ?? new Category { Id = 0, Name = "" };
        }

        public async Task<int> Category_Upsert(Category category)
        {
            await Task.Delay(1); // Simulate async operation
            var existingCategory = _categories.FirstOrDefault(c => c.Id == category.Id);
            if (existingCategory != null)
            {
                existingCategory.Name = category.Name;
                return 1;
            }
            else
            {
                category.Id = _categories.Any() ? _categories.Max(c => c.Id) + 1 : 1;
                _categories.Add(category);
                return category.Id;
            }
        }

        public async Task<int> Category_Delete(Category category)
        {
            await Task.Delay(1); // Simulate async operation
            var existingCategory = _categories.FirstOrDefault(c => c.Id == category.Id);
            if (existingCategory != null)
            {
                _categories.Remove(existingCategory);
                return 1;
            }
            return 0;
        }
    }
}
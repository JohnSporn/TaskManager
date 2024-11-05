using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TaskManager.Data.Models;
using TaskManager.Data.Repositories;

namespace TaskManager.Pages.Categories
{
    public class IndexModel : PageModel
    {
        private readonly ICategoryRepository _repository;

        public IndexModel(ICategoryRepository repository)
        {
            _repository = repository;
        }

        public IList<Category> Categories { get; set; } = new List<Category>();

        public async Task<IActionResult> OnGet()
        {
            await foreach(var item in _repository.Categories_Get())
            {
                Categories.Add(item);
            }
            return Page();
        }
    }
}

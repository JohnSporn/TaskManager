using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using TaskManager.Data.Models;
using TaskManager.Data.Repositories;

namespace TaskManager.Pages.Categories
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly ICategoryRepository _repository;
        private readonly ILogger _logger;

        public CreateModel(ICategoryRepository repository, ILogger<CreateModel> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public class InputModel
        {
            [Required]
            public required string Name { get; set; }
        };

        [BindProperty]
        public InputModel Input { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            var category = new Category
            {
                Name = Input.Name
            };
            await _repository.Category_Upsert(category);
            return RedirectToPage("/Categories/Index");
        }
    }
}

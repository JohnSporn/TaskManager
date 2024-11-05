using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using TaskManager.Data.Models;
using TaskManager.Data.Repositories;

namespace TaskManager.Pages.Categories
{
    public class EditModel : PageModel
    {
        private readonly ICategoryRepository _repository;
        private readonly ILogger _logger;

        public EditModel(ICategoryRepository repository, ILogger<CreateModel> logger)
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

        public async Task<IActionResult> OnGet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var category = await _repository.Categories_GetById(id);

            Input = new InputModel
            {
                Name = category.Name
            };
            if (Input == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPost(int id)
        {
            if (await TryUpdateModelAsync<InputModel>(
                Input,
                "input",
                i => i.Name))
            {
                var category = new Category
                {
                    Id = id,
                    Name = Input.Name
                };
                await _repository.Category_Upsert(category);
                return RedirectToPage("/Categories/Index");
            }
            return Page();
        }
    }
}

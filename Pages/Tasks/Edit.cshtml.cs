using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using TaskManager.Data.Models;
using TaskManager.Data.Repositories;

namespace TaskManager.Pages.Tasks
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly ITaskRepository _repository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ILogger _logger;

        public EditModel(ITaskRepository repository, ICategoryRepository categoryRepository, ILogger<CreateModel> logger)
        {
            _repository = repository;
            _categoryRepository = categoryRepository;
            _logger = logger;
        }

        public class InputModel
        {
            [Required]
            public required string Name { get; set; }
            [Required]
            public required DateTime DueDate { get; set; }
            [Required]
            public required Priority Priority { get; set; }
            [Required]
            public required Category Category { get; set; }
            [Required]
            public required bool IsComplete { get; set; }
        };

        [BindProperty]
        public InputModel Input { get; set; }
        public IList<Category> Categories { get; set; } = new List<Category>();

        public async Task<IActionResult> OnGet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var task = await _repository.Tasks_GetById(id);

            Input = new InputModel
            {
                Name = task.Name,
                DueDate = task.DueDate,
                Priority = task.Priority,
                Category = task.Category,
                IsComplete = task.IsComplete,
            };
            if (Input == null)
            {
                return NotFound();
            }
            await foreach (var item in _categoryRepository.Categories_Get())
            {
                Categories.Add(item);
            }
            return Page();
        }

        public async Task<IActionResult> OnPost(int id)
        {
            var task = new TaskItem
            {
                Id = id,
                Name = Input.Name,
                DueDate = Input.DueDate,
                Priority = Input.Priority,
                Category = Input.Category,
                IsComplete = Input.IsComplete
            };
            await _repository.Task_Upsert(task);
            return RedirectToPage("/Index");
        }
    }
}

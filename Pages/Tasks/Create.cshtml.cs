using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using TaskManager.Data.Models;
using TaskManager.Data.Repositories;

namespace TaskManager.Pages.Tasks
{
    public class CreateModel : PageModel
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ILogger _logger;

        public CreateModel(ITaskRepository taskRepository, ICategoryRepository categoryRepository, ILogger<CreateModel> logger)
        {
            _taskRepository = taskRepository;
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

        public async void OnGet()
        {
            await foreach(var item in _categoryRepository.Categories_Get())
            {
                Categories.Add(item);
            }
        }

        public async Task<IActionResult> OnPost()
        {
            var task = new TaskItem
            {
                Name = Input.Name,
                DueDate = Input.DueDate,
                Priority = Input.Priority,
                Category = Input.Category,
                IsComplete = Input.IsComplete
            };
            await _taskRepository.Task_Upsert(task);
            return RedirectToPage("/Tasks");
        }
    }
}

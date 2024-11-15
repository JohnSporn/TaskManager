using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using TaskManager.Data.Models;
using TaskManager.Data.Repositories;

namespace TaskManager.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ITaskRepository _repository;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ITaskRepository repository, IUserRepository userRepository, ILogger<IndexModel> logger)
        {
            _repository = repository;
            _userRepository = userRepository;
            _logger = logger;
        }

        public IList<TaskItem> Tasks { get; set; } = new List<TaskItem>();
        public IList<TaskItem> TasksCompleted { get; set; } = new List<TaskItem>();
        public IList<TaskItem> TasksPending { get; set; } = new List<TaskItem>();

        public async Task<IActionResult> OnGet()
        {
            var user = await _userRepository.Users_GetByName(User.Identity?.Name);
            if (user == null) 
            {
                var result = await _userRepository.User_Insert(User.Identity.Name);
                if(result == 0)
                {
                    return RedirectToPage("/Account/Logout");
                }
            }
            await foreach(var item in _repository.Tasks_Get())
            {
                Tasks.Add(item);
            }

            Tasks = Tasks.OrderByDescending(t => t.IsComplete).ToList();
            TasksCompleted = Tasks.Where(x => x.IsComplete == true).ToList();
            TasksPending = Tasks.Where(t => t.IsComplete == false).ToList();
            return Page();
        }
    }
}

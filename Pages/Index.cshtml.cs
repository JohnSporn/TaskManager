using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using TaskManager.Data.Models;
using TaskManager.Data.Repositories;

namespace TaskManager.Pages
{
//[Authorize]
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
            // Temporarily use a test user for development
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "test-user-id";
            var user = await _userRepository.Users_GetById(userId);
            if (user == null) 
            {
                var newUser = new User 
                {
                    Name = User.Identity?.Name ?? "Test User",
                    User_Id = userId
                };
                var result = await _userRepository.User_Insert(newUser);
                if(result == 0 && userId != "test-user-id")
                {
                    return RedirectToPage("/Account/Logout");
                }
                user = newUser;
            }
            await foreach(var item in _repository.Tasks_Get(user?.User_Id))
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

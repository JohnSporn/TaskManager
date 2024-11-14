using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TaskManager.Data.Repositories;

namespace TaskManager.Pages.Tasks
{
    public class DeleteModel : PageModel
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ILogger<DeleteModel> _logger;  

        public DeleteModel(ITaskRepository taskRepository, ILogger<DeleteModel> logger)
        {
            _taskRepository = taskRepository;
            _logger = logger;
        }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost(int id)
        {

        }
    }
}

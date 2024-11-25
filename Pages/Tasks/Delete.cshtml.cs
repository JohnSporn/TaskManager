using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using TaskManager.Data.Repositories;

namespace TaskManager.Pages.Tasks
{
    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ILogger<DeleteModel> _logger;

        public DeleteModel(ITaskRepository taskRepository, ILogger<DeleteModel> logger)
        {
            _taskRepository = taskRepository;
            _logger = logger;
        }

        public string Message { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await _taskRepository.Tasks_GetById(id, User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            if (task == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await _taskRepository.Tasks_GetById(id, User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            if (task == null)
            {
                return NotFound();
            }

            try
            {
                // Add userid.
                task.UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var result = await _taskRepository.Task_Delete(task);
                if(result == 0)
                {
                    Message = "There was an error deleting this task";
                    return Page();
                }
                return RedirectToPage("/Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                Message = "There was an error processing your request.";
                return RedirectToAction("/Tasks/Delete", new { id });
            }
        }
    }
}

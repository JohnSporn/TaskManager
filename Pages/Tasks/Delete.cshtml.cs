using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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

            var task = await _taskRepository.Tasks_GetById(id);

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

            var task = await _taskRepository.Tasks_GetById(id);

            if (task == null)
            {
                return NotFound();
            }

            try
            {
                await _taskRepository.Task_Delete(task);
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

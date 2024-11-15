using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TaskManager.Data.Repositories;

namespace TaskManager.Pages.Categories
{
    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly ICategoryRepository _repository;
        private readonly ILogger<DeleteModel> _logger;

        public DeleteModel(ICategoryRepository repository, ILogger<DeleteModel> logger)
        {
            _repository = repository;
            _logger = logger;
        }
        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _repository.Categories_GetById(id);

            if (category == null)
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

            var category = await _repository.Categories_GetById(id);

            if (category == null)
            {
                return NotFound();
            }

            try
            {
                await _repository.Category_Delete(category);
                return RedirectToPage("/Categories/Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ErrorMessage);

                return RedirectToAction("/Categories/Delete", new { id });
            }
        }
    }
}

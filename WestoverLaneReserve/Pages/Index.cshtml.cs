using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WestoverLaneReserve.Data;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace WestoverLaneReserve.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ApplicationDbContext _dbContext;

        public IndexModel(ILogger<IndexModel> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public void OnGet()
        {
        }

        public IActionResult OnPost(string email, string password)
        {
            var customer = _dbContext.Customers.SingleOrDefault(c => c.Email == email && c.Password == password);

            if (customer == null)
            {
                ViewData["ErrorMessage"] = "Wrong email or password";
                return Page();
            }

            // Authentication successful, redirect to LaneSchedule page
            return RedirectToPage("/LaneSchedule");
        }
    }
}
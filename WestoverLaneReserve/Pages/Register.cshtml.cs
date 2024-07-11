using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WestoverLaneReserve.Data;
using WestoverLaneReserve.Models;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WestoverLaneReserve.Pages
{
    public class RegisterModel : PageModel
    {

        private readonly ApplicationDbContext _context;
        private readonly ILogger<RegisterModel> _logger;

        public RegisterModel(ApplicationDbContext context, ILogger<RegisterModel> logger)
        {
            _context = context;
            _logger = logger;

            Customer = new Customer
            {
                FirstName = string.Empty,
                LastName = string.Empty,
                Email = string.Empty,
                Password = string.Empty
            };
        }

        [BindProperty]
        public Customer Customer { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Confirm password is required.")]
        public string ConfirmPassword { get; set; } = string.Empty;

        public void OnGet()
        {
            ViewData["ShowHeader"] = false; // Do not show header on this page
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (Customer.Password != ConfirmPassword)   // Server-side check for password matching
            {
                ModelState.AddModelError(string.Empty, "Passwords do not match.");
                return Page();
            }

            try
            {
                _context.Customers.Add(Customer);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Handle the exception (log it, display an error message, etc.)
                _logger.LogError(ex, "An error occurred while registering a new customer");
                ModelState.AddModelError(string.Empty, "An error occurred while saving your registration.");
                return Page(); // Or handle the error appropriately 
            }

            return RedirectToPage("/Index");
        }
    }
}
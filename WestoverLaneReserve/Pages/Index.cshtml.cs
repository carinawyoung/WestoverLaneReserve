using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WestoverLaneReserve.Data;
using Microsoft.Extensions.Logging;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using WestoverLaneReserve.Models;
using Microsoft.AspNetCore.Authentication;

namespace WestoverLaneReserve.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly SignInManager<CustomerApplicationUser> _signInManager;

        public IndexModel(ILogger<IndexModel> logger, SignInManager<CustomerApplicationUser> signInManager)
        {
            _logger = logger;
            _signInManager = signInManager;
        }

        public void OnGet()
        {
            ViewData["ShowHeader"] = false; // Do not show header on this page
        }

        public async Task<IActionResult> OnPostAsync(string email, string password)
        {

            // Check if the model state is valid
            if (!ModelState.IsValid)
            {
                // Add an error message or handle the error appropriately
                ModelState.AddModelError(string.Empty, "Please check your input and try again.");
                return Page(); // Return to the current page with validation summaries
            }

            var result = await _signInManager.PasswordSignInAsync(email, password, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return RedirectToPage("LaneSchedule");
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");

            return Page();
        }


        public async Task<IActionResult> OnPostLogoutAsync()
        {
            await HttpContext.SignOutAsync();
            return RedirectToPage("/Index");
        }
    }
}
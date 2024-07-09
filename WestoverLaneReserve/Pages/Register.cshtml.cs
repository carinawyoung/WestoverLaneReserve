using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WestoverLaneReserve.Data;
using WestoverLaneReserve.Models;

namespace WestoverLaneReserve.Pages
{
    public class RegisterModel : PageModel
    {

        private readonly ApplicationDbContext _context;

        public RegisterModel(ApplicationDbContext context)
        {
            _context = context;
            // Customer = new Customer();
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

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Customers.Add(Customer);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Index");
        }
    }

}
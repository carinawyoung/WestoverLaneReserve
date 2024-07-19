using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Globalization;
using System.Runtime.CompilerServices;
using WestoverLaneReserve.Data;
using WestoverLaneReserve.Models;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace WestoverLaneReserve.Pages
{

    public class MyReservationsModel : BasePageModel
    {
        private readonly ApplicationDbContext _context;  // Field for DbContext
        private readonly UserManager<CustomerApplicationUser> _userManager;  // Field to store UserManager

        public MyReservationsModel(ApplicationDbContext context, UserManager<CustomerApplicationUser> userManager) : base(userManager)  // pass userManager to the base model 
        {
            _context = context; // Assign parameter to the field
            _userManager = userManager; // Asign UserManager to the field
        }

        public IList<LaneReservation> Reservations { get; set; }  // Add a property to hold the reservations

        public async Task OnGetAsync()
        {
            ViewData["ShowHeader"] = true; // Do not show header on this page

            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                var today = DateTime.Today;
                Reservations = _context.LaneReservations
                                        .Where(r => r.CustomerId == user.Id)
                                        .ToList()
                                        .Where(r => DateTime.ParseExact(r.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture) >= today)
                                        .ToList();

            }

            await LoadUser(); // Load user information so name will be in header
        }

        // // If I wanted to use polymorphism to extend functionality
        // public string UserLastName { get; set; }
        // public new async Task LoadUser()
        // {
        //     var user = await UserManager.GetUserAsync(User);
        //     if (user != null)
        //     {
        //         UserFirstName = user.FirstName;
        //         UserLastName = user.LastName;
        //         ViewData["UserFirstName"] = UserFirstName;
        //         ViewData["UserLastName"] = UserLastName;
        //     }
        // }

        public async Task<IActionResult> OnPostCancelAsync(int id)
        {
            var reservation = await _context.LaneReservations.FindAsync(id);

            if (reservation != null)
            {
                _context.LaneReservations.Remove(reservation);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("/MyReservations");
        }

    }
}
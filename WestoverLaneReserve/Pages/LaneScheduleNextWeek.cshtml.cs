using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Globalization;
using System.Runtime.CompilerServices;
using WestoverLaneReserve.Data;
using WestoverLaneReserve.Models;

namespace WestoverLaneReserve.Pages
{

    public class LaneScheduleNextWeekModel : BasePageModel
    {
        private readonly ApplicationDbContext _context;  // Field for DbContext
        private readonly UserManager<CustomerApplicationUser> _userManager;  // Field to store UserManager

        public LaneScheduleNextWeekModel(ApplicationDbContext context, UserManager<CustomerApplicationUser> userManager) : base(userManager)  // pass userManager to the base model 
        {
            _context = context; // Assign parameter to the field
            _userManager = userManager; // Asign UserManager to the field
        }


        public List<string> NextWeekDates { get; private set; } = new List<string>();
        public List<string> Times { get; private set; } = new List<string>();
        public Dictionary<string, Dictionary<string, int>> LaneAvailability { get; private set; } = new Dictionary<string, Dictionary<string, int>>();


        public async Task OnGet()
        {
            ViewData["ShowHeader"] = true; // Do not show header on this page
            await LoadUser(); // Load user information

            NextWeekDates = GetNextWeekDates();
            Times = GetTimes();
            EnsureTimeSlotAvailability();  // Ensure time slots are intitalized 
            LoadLaneAvailability();        // Load existing or initialized availability

        }

        private void EnsureTimeSlotAvailability()
        {
            bool changesMade = false;

            foreach (var weekDate in NextWeekDates)
            {
                string formattedDate = DateTime.ParseExact(weekDate, "ddd M/d", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");

                foreach (var time in Times)
                {
                    string formattedTime = DateTime.ParseExact(time, "h:mm tt", CultureInfo.InvariantCulture).ToString("HH:mm");

                    // Check if entry already exists
                    var availability = _context.TimeSlotAvailabilities
                        .FirstOrDefault(t => t.Date == formattedDate && t.Time == formattedTime);

                    // If it does not exist, create and initialize it
                    if (availability == null)
                    {
                        _context.TimeSlotAvailabilities.Add(new TimeSlotAvailability
                        {
                            Date = formattedDate,
                            Time = formattedTime,
                            LanesAvailable = 6  // Initialize with 6 lanes available
                        });
                        changesMade = true;
                    }
                }
            }

            // Save changes if any new time slots were added
            if (changesMade)
            {
                _context.SaveChanges();
            }
        }
        private void LoadLaneAvailability()
        {
            foreach (var weekDate in NextWeekDates)
            {
                var dailyAvailability = new Dictionary<string, int>();

                foreach (var time in Times)
                {
                    string formattedDate = DateTime.ParseExact(weekDate, "ddd M/d", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
                    string formattedTime = DateTime.ParseExact(time, "h:mm tt", CultureInfo.InvariantCulture).ToString("HH:mm");

                    var availability = _context.TimeSlotAvailabilities
                        .FirstOrDefault(t => t.Date == formattedDate && t.Time == formattedTime);

                    dailyAvailability[formattedTime] = availability?.LanesAvailable ?? 0;  // Assume 0 if not found
                    LaneAvailability[formattedDate] = dailyAvailability;
                }
            }
        }

        private List<string> GetNextWeekDates()
        {
            List<string> weekdates = new List<string>();
            DateTime today = DateTime.Today;
            int daysUntilNextSunday = ((int)DayOfWeek.Sunday - (int)today.DayOfWeek + 7) % 7;
            if (daysUntilNextSunday == 0)
            {
                daysUntilNextSunday = 7; // This moves it to the next Sunday if today is Sunday
            }
            DateTime startOfNextWeek = today.AddDays(daysUntilNextSunday);

            for (int i = 0; i < 7; i++)
            {
                DateTime date = startOfNextWeek.AddDays(i);
                weekdates.Add(date.ToString("ddd M/d", CultureInfo.InvariantCulture));
            }

            return weekdates;
        }

        private List<string> GetTimes()
        {
            List<string> times = new List<string>(); // make a new empty list
            DateTime startTime = DateTime.Today.AddHours(6); //start time at 6:00 AM
            for (int i = 0; i < 12; i++)
            {
                times.Add(startTime.AddHours(i).ToString("h:mm tt", CultureInfo.InvariantCulture));
            }
            return times;
        }

        public async Task<IActionResult> OnPostAsync(string reserve)
        {
            // Split the reserve string to extract date and time
            var parts = reserve.Split(',');
            var date = parts[0];
            var time = parts[1];

            // Fetch the corresponding TimeSlotAvailability
            var timeSlotAvailability = _context.TimeSlotAvailabilities.FirstOrDefault(t => t.Date == date && t.Time == time);

            // Check if there are lanes available and decrement
            if (timeSlotAvailability != null && timeSlotAvailability.LanesAvailable > 0)
            {
                // Check for an existing reservation for this user, date, and time
                bool reservationExists = _context.LaneReservations.Any(r =>
                    r.CustomerId == _userManager.GetUserId(User) &&
                    r.Date == date &&
                    r.Time == time);

                if (reservationExists)
                {
                    TempData["ErrorMessage"] = "You already have a reservation for this date and time.";
                    return RedirectToPage();
                }
                else
                {
                    // Create a new reservation
                    var reservation = new LaneReservation
                    {
                        CustomerId = _userManager.GetUserId(User), // uses the private field _userManager
                        Date = date,
                        Time = time
                    };

                    // Add to the database
                    _context.LaneReservations.Add(reservation);

                    // Decrement the number of available lanes
                    timeSlotAvailability.LanesAvailable -= 1;

                    // Save all the changes to the database
                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToPage();
        }
    }
}
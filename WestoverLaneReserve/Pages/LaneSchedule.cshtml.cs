using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Globalization;
using System.Runtime.CompilerServices;
using WestoverLaneReserve.Data;
using WestoverLaneReserve.Models;

namespace WestoverLaneReserve.Pages
{
    public class LaneScheduleModel : PageModel
    {
        private readonly ApplicationDbContext _context;  // Field for DbContext

        public LaneScheduleModel(ApplicationDbContext context)  // 
        {
            _context = context; // Assign parameter to the field
        }


        public List<string> WeekDates { get; private set; } = new List<string>();
        public List<string> Times { get; private set; } = new List<string>();
        public Dictionary<string, Dictionary<string, int>> LaneAvailability { get; private set; } = new Dictionary<string, Dictionary<string, int>>();


        public void OnGet()
        {
            ViewData["ShowHeader"] = true; // Do not show header on this page

            WeekDates = GetCurrentWeekDates();
            Times = GetTimes();
            EnsureTimeSlotAvailability();  // Ensure time slots are intitalized 
            LoadLaneAvailability();        // Load existing or initialized availability

        }

        private void EnsureTimeSlotAvailability()
        {
            bool changesMade = false;

            foreach (var weekDate in WeekDates)
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
            foreach (var weekDate in WeekDates)
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
        //     // Create TimeSLotAvailability entries if they don't already exist
        //     foreach (var weekDate in WeekDates)
        //     {
        //         foreach (var time in Times)
        //         {
        //             // DateTime date = DateTime.ParseExact(weekDate, "ddd M/d", CultureInfo.InvariantCulture);
        //             // DateTime timeOfDay = DateTime.ParseExact(time, "h:mm tt", CultureInfo.InvariantCulture);

        //             string formattedDate = DateTime.ParseExact(weekDate, "ddd M/d", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
        //             string formattedTime = DateTime.ParseExact(time, "h:mm tt", CultureInfo.InvariantCulture).ToString("HH:mm");


        //             // Check if entry already exists
        //             if (!_context.TimeSlotAvailabilities.Any(t =>
        //                 // t.Date.Date == date.Date && t.Time.TimeOfDay == timeOfDay.TimeOfDay))
        //                 t.Date == formattedDate && t.Time == formattedTime))
        //             {
        //                 // Create new TimeSlotAvailability entry
        //                 var availability = new TimeSlotAvailability
        //                 {
        //                     Date = formattedDate,
        //                     Time = formattedTime,
        //                     // Date = dateTime,
        //                     // Time = timeOfDay,
        //                     LanesAvailable = 6  // There are six lanes at the pool
        //                 };

        //                 _context.TimeSlotAvailabilities.Add(availability);
        //             }
        //         }
        //     }

        //     _context.SaveChanges();   // Save changes to the database
        // }

        private List<string> GetCurrentWeekDates()
        {
            List<string> weekdates = new List<string>();
            DateTime today = DateTime.Today;
            int daysUntilSunday = (int)DayOfWeek.Sunday - (int)today.DayOfWeek;
            DateTime startOfWeek = today.AddDays(daysUntilSunday);

            for (int i = 0; i < 7; i++)
            {
                DateTime date = startOfWeek.AddDays(i);
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
    }
}
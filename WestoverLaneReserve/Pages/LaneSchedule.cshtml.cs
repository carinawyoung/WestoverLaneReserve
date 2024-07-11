using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Globalization;
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


        public List<string> WeekDates { get; private set; } = new List<string>();  // Is this correct? SHould these be Dates instead of strings?
        public List<string> Times { get; private set; } = new List<string>();

        public void OnGet()
        {
            WeekDates = GetCurrentWeekDates();
            Times = GetTimes();

            // Create TimeSLotAvailability entries if they don't already exist
            foreach (var weekDate in WeekDates)
            {
                foreach (var time in Times)
                {
                    // DateTime date = DateTime.ParseExact(weekDate, "ddd M/d", CultureInfo.InvariantCulture);
                    // DateTime timeOfDay = DateTime.ParseExact(time, "h:mm tt", CultureInfo.InvariantCulture);

                    string formattedDate = DateTime.ParseExact(weekDate, "ddd M/d", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
                    string formattedTime = DateTime.ParseExact(time, "h:mm tt", CultureInfo.InvariantCulture).ToString("HH:mm");


                    // Check if entry already exists
                    if (!_context.TimeSlotAvailabilities.Any(t =>
                        // t.Date.Date == date.Date && t.Time.TimeOfDay == timeOfDay.TimeOfDay))
                        t.Date == formattedDate && t.Time == formattedTime))
                    {
                        // Create new TimeSlotAvailability entry
                        var availability = new TimeSlotAvailability
                        {
                            Date = formattedDate,
                            Time = formattedTime,
                            // Date = dateTime,
                            // Time = timeOfDay,
                            LanesAvailable = 6  // There are six lanes at the pool
                        };

                        _context.TimeSlotAvailabilities.Add(availability);
                    }
                }
            }

            _context.SaveChanges();   // Save changes to the database
        }

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
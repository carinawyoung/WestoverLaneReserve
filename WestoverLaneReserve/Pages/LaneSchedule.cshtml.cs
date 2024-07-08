using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Globalization;

namespace WestoverLaneReserve.Pages
{
    public class LaneScheduleModel : PageModel
    {
        public List<string> WeekDates { get; private set; } = new List<string>();
        public List<string> Times { get; private set; } = new List<string>();

        public void OnGet()
        {
            WeekDates = GetCurrentWeekDates();
            Times = GetTimes();
        }

        private static List<string> GetCurrentWeekDates()
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
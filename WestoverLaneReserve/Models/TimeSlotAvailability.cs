using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WestoverLaneReserve.Models
{
    public class TimeSlotAvailability
    {
        [Key]
        [Column(Order = 1)]
        //[Column(Order = 1, TypeName = "Date")]
        //[Column(TypeName ="Date")] // I want it to store only the date part of the datetime
        // public DateTime Date { get; set; }  // Composite key part 1
        public string Date { get; set; }

        [Key]
        [Column(Order = 2)]
        public string Time { get; set; }
        //[Column(Order = 2, TypeName = "Time")]
        //public DateTime Time { get; set; }  // Composite key part 2

        public int LanesAvailable { get; set; }  // Number lanes available for this Date and Time slot
    }
}

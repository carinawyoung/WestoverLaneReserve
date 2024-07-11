using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WestoverLaneReserve.Models
{
    public class TimeSlotAvailability
    {
        [Key]
        [Column(Order = 1)]
        public DateTime Date { get; set; }  // Composite key part 1

        [Key]
        [Column(Order = 2)]
        public DateTime Time { get; set; }  // Composite key part 2

        public int LanesAvailable { get; set; }  // Number lanes available for this Date and Time slot
    }
}

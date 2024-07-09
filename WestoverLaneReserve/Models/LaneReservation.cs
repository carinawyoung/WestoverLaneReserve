using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WestoverLaneReserve.Models
{
    public class LaneReservation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReservationId { get; private set; }
        public int CustomerId { get; set; }
        public DateTime Date { get; set; }
        public DateTime Time { get; set; }

    }
}
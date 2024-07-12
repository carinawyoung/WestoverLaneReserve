using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WestoverLaneReserve.Models
{
    public class LaneReservation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReservationId { get; private set; }
        [ForeignKey("CustomerApplicationUser")]
        public string CustomerId { get; set; }
        public CustomerApplicationUser CustomerApplicationUser { get; set; }
        public DateTime Date { get; set; }
        public DateTime Time { get; set; }

    }
}
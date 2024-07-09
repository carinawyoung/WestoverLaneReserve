namespace WestoverLaneReserve.Models
{
    public class LaneReservation
    {
        public int ReservationId { get; private set; }
        public int CustomerId { get; set; }
        public DateTime Date { get; set; }
        public DateTime Time { get; set; }

    }
}
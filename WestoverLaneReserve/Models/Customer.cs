namespace WestoverLaneReserve.Models
{
    public class Customer
    {
        public int CustomerId { get; private set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }

    }
}
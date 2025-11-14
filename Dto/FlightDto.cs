namespace FlightsReservation.Dto
{
    public class FlightDto
    {
        public int Id { get; set; }

        public string DepartureCity { get; set; } = string.Empty;

        public string ArrivalCity { get; set; } = string.Empty;

        public DateTime FlightDate { get; set; }

        public int NumberOfStops { get; set; }

        public int TotalSeats { get; set; }

        public int AvailableSeats { get; set; }
        public string Status { get; set; } = "Active";
    }
}

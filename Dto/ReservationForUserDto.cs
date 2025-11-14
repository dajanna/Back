namespace FlightsReservation.Dto
{
    public class ReservationForUserDto
    {
        public int Id { get; set; }
        public int SeatsReserved { get; set; }
        public DateTime ReservationDate { get; set; }
        public string StatusName { get; set; } = string.Empty;
        public int FlightId { get; set; }

        public FlightDto Flight { get; set; } = new FlightDto();
    }
}

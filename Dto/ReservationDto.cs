namespace FlightsReservation.Dto
{
    public class ReservationDto
    {
        public int Id { get; set; }
        public int SeatsReserved { get; set; }
        public DateTime ReservationDate { get; set; }
        public int StatusId { get; set; }
        public int FlightId { get; set; }
        public string UserId { get; set; } = string.Empty;
       
    }
}

namespace FlightsReservation.Dto
{
    public class ReservationForAgentDto
    {
        public int Id { get; set; }
        public int FlightId { get; set; }
        public string UserName { get; set; } 
        public int SeatsReserved { get; set; }
        public DateTime ReservationDate { get; set; }
        public string StatusName { get; set; } 
    }
}

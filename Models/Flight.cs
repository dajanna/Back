using System.ComponentModel.DataAnnotations;

namespace FlightsReservation.Models
{
    public class Flight
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string DepartureCity { get; set; } = string.Empty;

        [Required]
        public string ArrivalCity { get; set; } = string.Empty;

        [Required]
        public DateTime FlightDate { get; set; }

        [Required]
        public int NumberOfStops { get; set; }

        [Required]
        public int TotalSeats { get; set; }

        [Required]
        public int AvailableSeats { get; set; }
        [Required]
        public string Status { get; set; } = "Active";

        public ICollection<Reservation>? Reservations { get; set; }
    }
}

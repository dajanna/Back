using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlightsReservation.Models
{
    public class Reservation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int SeatsReserved { get; set; }

        [Required]
        public DateTime ReservationDate { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        [ForeignKey(nameof(UserId))]
        public AppUser? User { get; set; }

        [Required]
        public int FlightId { get; set; }

        [ForeignKey(nameof(FlightId))]
        public Flight? Flight { get; set; }

        [Required]
        public int StatusId { get; set; }

        [ForeignKey(nameof(StatusId))]
        public Status? Status { get; set; }
    }
}

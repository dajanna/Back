using System.ComponentModel.DataAnnotations;

namespace FlightsReservation.Models
{
    public class Status
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

      
        public ICollection<Reservation>? Reservations { get; set; }
    }
}

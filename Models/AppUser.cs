using Microsoft.AspNetCore.Identity;

namespace FlightsReservation.Models
{
    public class AppUser : IdentityUser
    {
        public string? FullName { get; set; }
        public ICollection<Reservation>? Reservations { get; set; }
    }
}

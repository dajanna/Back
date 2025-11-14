using System.ComponentModel.DataAnnotations;

namespace FlightsReservation.Dto
{
    public class LogInDto
    {
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}

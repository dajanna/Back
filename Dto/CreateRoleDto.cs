using System.ComponentModel.DataAnnotations;

namespace FlightsReservation.Dto
{
    public class CreateRoleDto
    {
        [Required(ErrorMessage = "Role name is required")]
        public string RoleName { get; set; } = null;
    }
}

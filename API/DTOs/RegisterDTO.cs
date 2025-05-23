using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class RegisterDTO
    {
        [Required]
        [MaxLength(25)]
        public required string Username { get; set; }
        [Required]

        public required string Password { get; set; }
    }
}

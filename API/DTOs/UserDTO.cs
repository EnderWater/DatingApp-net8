using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class UserDTO
    {
        public required string UserName { get; set; }
        public required string Token { get; set; }
    }
}

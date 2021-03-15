using System.ComponentModel.DataAnnotations;

namespace API.DTO.v1.Identity
{
    /// <summary>
    /// DTO for sending user email and password to log in.
    /// </summary>
    public class LoginDto
    {
        [Required]
        public string Email { get; set; } = default!;
        
        [Required]
        public string Password { get; set; } = default!;
    }
}
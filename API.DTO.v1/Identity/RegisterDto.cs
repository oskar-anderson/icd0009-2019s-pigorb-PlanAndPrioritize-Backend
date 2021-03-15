using System.ComponentModel.DataAnnotations;

namespace API.DTO.v1.Identity
{
    /// <summary>
    /// DTO to register new user.
    /// </summary>
    public class RegisterDto
    {
        [MinLength(5)] [MaxLength(128)] [Required]
        public string Email { get; set; } = default!;
        
        [MinLength(5)] [MaxLength(64)] [Required]
        public string Password { get; set; } = default!;
        
        [MinLength(1)] [MaxLength(255)] [Required]
        public string FirstName { get; set; } = default!;
        
        [MinLength(1)] [MaxLength(255)] [Required]
        public string LastName { get; set; } = default!;
    }
}
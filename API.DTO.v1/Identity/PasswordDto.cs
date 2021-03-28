using System.ComponentModel.DataAnnotations;

namespace API.DTO.v1.Identity
{
    /// <summary>
    /// Change user password.
    /// </summary>
    public class PasswordDto
    {
        [MinLength(5)] [MaxLength(128)] [Required]
        public string Email { get; set; } = default!;
        
        [MinLength(5)] [MaxLength(64)] [Required]
        public string OldPassword { get; set; } = default!;
        
        [MinLength(5)] [MaxLength(64)] [Required]
        public string NewPassword { get; set; } = default!;
    }
}
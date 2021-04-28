using System.ComponentModel.DataAnnotations;

namespace API.DTO.v1.Identity
{
    /// <summary>
    /// Reset user password.
    /// </summary>
    public class ResetPasswordDto
    {
        [MinLength(5)] [MaxLength(128)] [Required]
        public string Email { get; set; } = default!;

        [MinLength(5)] [MaxLength(64)] [Required]
        public string Password { get; set; } = default!;
    }
}
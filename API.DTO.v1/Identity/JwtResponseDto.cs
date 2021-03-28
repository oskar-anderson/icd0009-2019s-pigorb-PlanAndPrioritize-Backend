using System.ComponentModel.DataAnnotations;

namespace API.DTO.v1.Identity
{
    /// <summary>
    /// DTO to send Json Web Token.
    /// </summary>
    public class JwtResponseDto
    {
        [Required]
        public string Token { get; set; } = default!;
        
        [Required]
        public string Status { get; set; } = default!;

        [Required]
        public bool RequirePasswordChange { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace API.DTO.v1.UserManager
{
    public class UserCreateDto
    {
        [MinLength(5)] [MaxLength(128)]
        public string Email { get; set; } = default!;
        
        [MinLength(5)] [MaxLength(64)]
        public string Password { get; set; } = default!;
        
        [MinLength(1)] [MaxLength(255)]
        public string FirstName { get; set; } = default!;
        
        [MinLength(1)] [MaxLength(255)]
        public string LastName { get; set; } = default!;
        
        [MinLength(1)] [MaxLength(64)]
        public string RoleName { get; set; } = default!;
    }
}
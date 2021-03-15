using System;
using System.ComponentModel.DataAnnotations;

namespace API.DTO.v1.UserManager
{
    public class UserIndexDto
    {
        public Guid Id { get; set; } = default!;
        
        [MinLength(5)] [MaxLength(128)]
        public string Email { get; set; } = default!;
        
        [MinLength(1)] [MaxLength(255)]
        public string FirstLastName { get; set; } = default!;
        
        [MinLength(1)] [MaxLength(64)]
        public string Roles { get; set; } = default!;
    }
}
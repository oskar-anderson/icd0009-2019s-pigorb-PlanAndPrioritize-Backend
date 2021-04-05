using System;

namespace API.DTO.v1
{
    public class AppUserApiDto
    {
        public Guid Id { get; set; }
        
        public string FirstName { get; set; } = default!;
        
        public string LastName { get; set; } = default!;
        
        public string Email { get; set; } = default!;
    }
}
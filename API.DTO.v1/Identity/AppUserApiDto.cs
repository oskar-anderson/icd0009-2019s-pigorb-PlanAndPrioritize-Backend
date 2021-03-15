using System;
using System.ComponentModel.DataAnnotations;

namespace API.DTO.v1.Identity
{
    /// <summary>
    /// DTO to create a new AppUser object.
    /// </summary>
    public class AppUserApiDtoCreate
    {
        [MaxLength(255)] [MinLength(1)] public string FirstName { get; set; } = default!;

        [MaxLength(255)] [MinLength(1)] public string LastName { get; set; } = default!;
        
        public string Email { get; set; } = default!;
        public string UserName { get; set; } = default!;
    }
    
    /// <summary>
    /// DTO to edit AppUser.
    /// </summary>
    public class AppUserApiDtoEdit : AppUserApiDtoCreate
    {
        public Guid Id { get; set; }
    }
    
    /// <summary>
    /// DTO to get AppUser data.
    /// </summary>
    public class AppUserApiDto : AppUserApiDtoEdit
    {
    }
}
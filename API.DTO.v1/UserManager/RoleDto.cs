using System.ComponentModel.DataAnnotations;

namespace API.DTO.v1.UserManager
{
    public class RoleDto
    {
        [MaxLength(64)]
        public string RoleName { get; set; } = default!;
    }
}
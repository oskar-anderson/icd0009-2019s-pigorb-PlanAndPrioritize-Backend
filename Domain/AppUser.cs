using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Domain
{
    public class AppUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; } = default!;
        
        public string LastName { get; set; } = default!;
        
        public ICollection<Comment>? Comments { get; set; }

        public ICollection<Feature>? Features { get; set; }
        
        public ICollection<UserInVoting>? UserInVotings { get; set; }
        
        public ICollection<UsersFeaturePriority>? UsersFeaturePriorities { get; set; }
    }
}

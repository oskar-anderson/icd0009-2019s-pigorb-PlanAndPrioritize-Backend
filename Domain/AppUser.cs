using System;
using System.Collections.Generic;
using ee.itcollege.pigorb.bookswap.Contracts.DAL.Base;
using Microsoft.AspNetCore.Identity;

namespace Domain
{
    public class AppUser : IdentityUser<Guid>, IDomainBaseEntity<Guid>
    {
        public string FirstName { get; set; } = default!;
        
        public string LastName { get; set; } = default!;
        
        public ICollection<Comment>? Comments { get; set; }

        public ICollection<Feature>? Features { get; set; }
        
        public ICollection<UserInVoting>? UserInVotings { get; set; }
        
        public ICollection<UsersFeaturePriority>? UsersFeaturePriorities { get; set; }
    }
}

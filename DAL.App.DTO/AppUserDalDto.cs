using System;
using ee.itcollege.pigorb.bookswap.Contracts.DAL.Base;

namespace DAL.App.DTO
{
    public class AppUserDalDto : IDomainBaseEntity<Guid>
    {
        public Guid Id { get; set; }
        
        public string FirstName { get; set; } = default!;
        
        public string LastName { get; set; } = default!;
        
        // public ICollection<CommentDalDto>? Comments { get; set; }
        //
        // public ICollection<FeatureDalDto>? Features { get; set; }
        //
        // public ICollection<UserInVotingDalDto>? UserInVotings { get; set; }
        //
        // public ICollection<UsersFeaturePriorityDalDto>? UsersFeaturePriorities { get; set; }
    }
}
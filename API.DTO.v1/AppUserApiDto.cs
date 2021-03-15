using System;
using ee.itcollege.pigorb.bookswap.Contracts.DAL.Base;

namespace API.DTO.v1
{
    public class AppUserApiDto : IDomainBaseEntity<Guid> // Duplicate
    {
        public Guid Id { get; set; }
        
        public string FirstName { get; set; } = default!;
        
        public string LastName { get; set; } = default!;
        
        // public ICollection<CommentApiDto>? Comments { get; set; }
        //
        // public ICollection<FeatureApiDto>? Features { get; set; }
        //
        // public ICollection<UserInVotingApiDto>? UserInVotings { get; set; }
        //
        // public ICollection<UsersFeaturePriorityApiDto>? UsersFeaturePriorities { get; set; }
    }
}
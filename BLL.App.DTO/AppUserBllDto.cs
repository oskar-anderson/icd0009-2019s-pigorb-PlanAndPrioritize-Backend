using System;
using System.Collections.Generic;
using ee.itcollege.pigorb.bookswap.Contracts.DAL.Base;

namespace BLL.App.DTO
{
    public class AppUserBllDto : IDomainBaseEntity<Guid>
    {
        public Guid Id { get; set; }
        
        public string FirstName { get; set; } = default!;
        
        public string LastName { get; set; } = default!;
        
        public string Email { get; set; } = default!;
        
        public ICollection<UserInVotingBllDto>? UserInVotings { get; set; }
    }
}
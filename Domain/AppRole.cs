using System;
using ee.itcollege.pigorb.bookswap.Contracts.DAL.Base;
using Microsoft.AspNetCore.Identity;

namespace Domain
{
    public class AppRole : IdentityRole<Guid>, IDomainBaseEntity<Guid>
    {
        
    }
}

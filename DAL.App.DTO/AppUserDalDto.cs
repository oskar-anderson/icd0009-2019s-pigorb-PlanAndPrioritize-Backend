using System;
using ee.itcollege.pigorb.bookswap.Contracts.DAL.Base;

namespace DAL.App.DTO
{
    public class AppUserDalDto : IDomainBaseEntity<Guid>
    {
        public Guid Id { get; set; }
    }
}
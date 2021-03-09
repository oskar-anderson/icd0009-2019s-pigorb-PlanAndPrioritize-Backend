using System;
using ee.itcollege.pigorb.bookswap.Contracts.DAL.Base;

namespace API.DTO.v1
{
    public class VotingStatusApiDto : IDomainBaseEntity<Guid>
    {
        public Guid Id { get; set; }
        
        public string Name { get; set; } = default!;

        public string? Description { get; set; }

        //public ICollection<VotingApiDto>? Votings { get; set; }
    }
}
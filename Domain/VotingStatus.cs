using System;
using System.Collections.Generic;

namespace Domain
{
    public class VotingStatus
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = default!;

        public string? Description { get; set; }

        public ICollection<Voting>? Votings { get; set; }
    }
}

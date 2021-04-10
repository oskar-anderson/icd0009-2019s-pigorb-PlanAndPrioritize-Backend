using System;
using System.Collections.Generic;

namespace API.DTO.v1
{
    public class VotingCreateApiDto
    {
        public string Title { get; set; } = default!;

        public string? Description { get; set; }

        public DateTime StartTime { get; set; }
        
        public DateTime EndTime { get; set; }
        public ICollection<Guid> Users { get; set; } = default!;
        
        public ICollection<Guid> Features { get; set; } = default!;
    }

    public class VotingEditApiDto : VotingCreateApiDto
    {
        public Guid Id { get; set; }
        public string VotingStatus { get; set; } = default!;
    }

    public class VotingApiDto
    {
        public Guid Id { get; set; }
        
        public string Title { get; set; } = default!;

        public string? Description { get; set; }

        public DateTime StartTime { get; set; }
        
        public DateTime EndTime { get; set; }
        
        public string VotingStatus { get; set; } = default!;
    }
}
using System;

namespace API.DTO.v1
{
    public class VotingCreateApiDto
    {
        public string Title { get; set; } = default!;

        public string? Description { get; set; }

        public DateTime StartTime { get; set; }
        
        public DateTime EndTime { get; set; }
    }

    public class VotingApiDto : VotingCreateApiDto
    {
        public Guid Id { get; set; }
        
        public string VotingStatus { get; set; } = default!;
    }
}
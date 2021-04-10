using System;

namespace API.DTO.v1
{
    public class UserInVotingApiDto
    {
        public Guid Id { get; set; }
        
        public Guid AppUserId { get; set; }

        public Guid VotingId { get; set; }
    }
    
    public class UserInVotingCreateApiDto
    {
        public Guid AppUserId { get; set; }

        public Guid VotingId { get; set; }
    }
}
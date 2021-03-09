using System;
using ee.itcollege.pigorb.bookswap.Contracts.DAL.Base;

namespace API.DTO.v1
{
    public class FeatureApiDto : IDomainBaseEntity<Guid>
    {
        public Guid Id { get; set; }
        
        public string Title { get; set; } = default!;
        
        public int Size { get; set; }

        public decimal PriorityValue { get; set; }

        public string? Description { get; set; }
        
        public DateTime StartTime { get; set; }
        
        public DateTime EndTime { get; set; }

        public int Duration { get; set; }

        public Guid? CategoryId { get; set; }
        // public CategoryApiDto? Category { get; set; }
        
        public Guid FeatureStatusId { get; set; }
        // public FeatureStatusApiDto? FeatureStatus { get; set; }
        
        public Guid? AppUserId { get; set; }
        //public AppUserApiDto? AppUser { get; set; }

        public DateTime TimeCreated { get; set; }
        
        public DateTime LastEdited { get; set; }

        public string? ChangeLog { get; set; }

        // public ICollection<CommentApiDto>? Comments { get; set; }
        //
        // public ICollection<FeatureInVotingApiDto>? FeatureInVotings { get; set; }
    }
}
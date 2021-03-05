using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ee.itcollege.pigorb.bookswap.Contracts.DAL.Base;

namespace Domain
{
    public class Feature : IDomainBaseEntity<Guid>
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = default!;
        
        public int Size { get; set; }

        public decimal PriorityValue { get; set; }

        public string? Description { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime StartTime { get; set; }
        
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime EndTime { get; set; }

        public int Duration { get; set; }

        public Guid? CategoryId { get; set; }
        public Category? Category { get; set; }
        
        public Guid FeatureStatusId { get; set; }
        public FeatureStatus? FeatureStatus { get; set; }
        
        public Guid? AppUserId { get; set; }
        public AppUser? AppUser { get; set; }

        public DateTime TimeCreated { get; set; }
        
        public DateTime LastEdited { get; set; }

        public string? ChangeLog { get; set; }

        public ICollection<Comment>? Comments { get; set; }
        
        public ICollection<FeatureInVoting>? FeatureInVotings { get; set; }
    }
}

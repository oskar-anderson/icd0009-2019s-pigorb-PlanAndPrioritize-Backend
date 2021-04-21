using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Classifiers;
using ee.itcollege.pigorb.bookswap.Contracts.DAL.Base;

namespace Domain
{
    public class Feature : IDomainBaseEntity<Guid>
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = default!;
        
        public int Size { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal PriorityValue { get; set; }

        public string? Description { get; set; }

        [DataType(DataType.Date)]
        public DateTime? StartTime { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime? EndTime { get; set; }

        public int Duration { get; set; }

        public Guid CategoryId { get; set; }
        public Category? Category { get; set; }

        public FeatureStatus FeatureStatus { get; set; }

        public Guid? AppUserId { get; set; }
        
        [InverseProperty("Features")]
        public AppUser? AppUser { get; set; }

        public DateTime TimeCreated { get; set; }
        
        public Guid CreatedById { get; set; }
        
        [InverseProperty("FeaturesCreated")]
        public AppUser? CreatedBy { get; set; }

        public DateTime LastEdited { get; set; }

        public string? ChangeLog { get; set; }

        public ICollection<Comment>? Comments { get; set; }
        
        public ICollection<FeatureInVoting>? FeatureInVotings { get; set; }
    }
}

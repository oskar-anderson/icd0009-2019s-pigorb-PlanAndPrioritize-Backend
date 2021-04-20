using System;
using System.Collections.Generic;
using Classifiers;
using ee.itcollege.pigorb.bookswap.Contracts.DAL.Base;

namespace BLL.App.DTO
{
    public class FeatureBllDto : IDomainBaseEntity<Guid>
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = default!;
        public int Size { get; set; }
        public decimal PriorityValue { get; set; }
        public string? Description { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int Duration { get; set; }
        public Guid CategoryId { get; set; }
        public CategoryBllDto? Category { get; set; }
        public FeatureStatus FeatureStatus { get; set; }
        public Guid? AppUserId { get; set; }
        public AppUserBllDto? AppUser { get; set; }
        public DateTime TimeCreated { get; set; }

        public Guid CreatedById { get; set; }
        public AppUserBllDto? CreatedBy { get; set; }
        public DateTime LastEdited { get; set; }
        public string? ChangeLog { get; set; }
        public ICollection<CommentBllDto>? Comments { get; set; }
        public ICollection<FeatureInVotingBllDto>? FeatureInVotings { get; set; }
        public bool IsInOpenVoting { get; set; }

        protected bool Equals(FeatureBllDto other)
        {
            return Id.Equals(other.Id) && Title == other.Title && Size == other.Size &&
                   PriorityValue == other.PriorityValue && Description == other.Description &&
                   Nullable.Equals(StartTime, other.StartTime) && Nullable.Equals(EndTime, other.EndTime) &&
                   Duration == other.Duration && CategoryId.Equals(other.CategoryId) &&
                   Equals(Category, other.Category) && FeatureStatus == other.FeatureStatus &&
                   Nullable.Equals(AppUserId, other.AppUserId) && Equals(AppUser, other.AppUser) &&
                   TimeCreated.Equals(other.TimeCreated) && CreatedById.Equals(other.CreatedById) &&
                   Equals(CreatedBy, other.CreatedBy) && LastEdited.Equals(other.LastEdited) &&
                   ChangeLog == other.ChangeLog && Equals(Comments, other.Comments) &&
                   Equals(FeatureInVotings, other.FeatureInVotings) && IsInOpenVoting == other.IsInOpenVoting;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((FeatureBllDto) obj);
        }

        public override int GetHashCode()
        {
            var hashCode = new HashCode();
            hashCode.Add(Id);
            hashCode.Add(Title);
            hashCode.Add(Size);
            hashCode.Add(PriorityValue);
            hashCode.Add(Description);
            hashCode.Add(StartTime);
            hashCode.Add(EndTime);
            hashCode.Add(Duration);
            hashCode.Add(CategoryId);
            hashCode.Add(Category);
            hashCode.Add((int) FeatureStatus);
            hashCode.Add(AppUserId);
            hashCode.Add(AppUser);
            hashCode.Add(TimeCreated);
            hashCode.Add(CreatedById);
            hashCode.Add(CreatedBy);
            hashCode.Add(LastEdited);
            hashCode.Add(ChangeLog);
            hashCode.Add(Comments);
            hashCode.Add(FeatureInVotings);
            hashCode.Add(IsInOpenVoting);
            return hashCode.ToHashCode();
        }
    }

    public class FeatureWithUsersPriorityBllDto : IDomainBaseEntity<Guid>
    {
        public Guid Id { get; set; }

        public Guid VotingId { get; set; } = default!;

        public string Title { get; set; } = default!;

        public string? Description { get; set; }

        public string CategoryName { get; set; } = default!;

        public int TaskSize { get; set; }

        public int BusinessValue { get; set; }

        public int TimeCriticality { get; set; }

        public int RiskOrOpportunity { get; set; }

        protected bool Equals(FeatureWithUsersPriorityBllDto other)
        {
            return Id.Equals(other.Id) && VotingId.Equals(other.VotingId) && Title == other.Title &&
                   Description == other.Description && CategoryName == other.CategoryName &&
                   TaskSize == other.TaskSize && BusinessValue == other.BusinessValue &&
                   TimeCriticality == other.TimeCriticality && RiskOrOpportunity == other.RiskOrOpportunity;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((FeatureWithUsersPriorityBllDto) obj);
        }

        public override int GetHashCode()
        {
            var hashCode = new HashCode();
            hashCode.Add(Id);
            hashCode.Add(VotingId);
            hashCode.Add(Title);
            hashCode.Add(Description);
            hashCode.Add(CategoryName);
            hashCode.Add(TaskSize);
            hashCode.Add(BusinessValue);
            hashCode.Add(TimeCriticality);
            hashCode.Add(RiskOrOpportunity);
            return hashCode.ToHashCode();
        }
    }
}
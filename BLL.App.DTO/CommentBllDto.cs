using System;
using ee.itcollege.pigorb.bookswap.Contracts.DAL.Base;

namespace BLL.App.DTO
{
    public class CommentBllDto : IDomainBaseEntity<Guid>
    {
        public Guid Id { get; set; }

        public string Content { get; set; } = default!;

        public Guid FeatureId { get; set; } = default!;
        public FeatureBllDto? Feature { get; set; }

        public Guid AppUserId { get; set; } = default!;
        public AppUserBllDto? AppUser { get; set; }

        public DateTime TimeCreated { get; set; }

        protected bool Equals(CommentBllDto other)
        {
            return Id.Equals(other.Id) && Content == other.Content && FeatureId.Equals(other.FeatureId) &&
                   Equals(Feature, other.Feature) && AppUserId.Equals(other.AppUserId) &&
                   Equals(AppUser, other.AppUser) && TimeCreated.Equals(other.TimeCreated);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((CommentBllDto) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Content, FeatureId, Feature, AppUserId, AppUser, TimeCreated);
        }
    }
}
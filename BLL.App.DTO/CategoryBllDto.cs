using System;
using ee.itcollege.pigorb.bookswap.Contracts.DAL.Base;

namespace BLL.App.DTO
{
    public class CategoryEditBllDto : IDomainBaseEntity<Guid>
    {
        public Guid Id { get; set; }
        
        public string Title { get; set; } = default!;
        
        public string? Description { get; set; }

        protected bool Equals(CategoryEditBllDto other)
        {
            return Id.Equals(other.Id) && Title == other.Title && Description == other.Description;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((CategoryEditBllDto) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Title, Description);
        }
    }
    
    public class CategoryBllDto : CategoryEditBllDto
    {
        public int Count { get; set; }
        
        public int InProgress { get; set; }
        
        public int Finished { get; set; }

        protected bool Equals(CategoryBllDto other)
        {
            return Count == other.Count && InProgress == other.InProgress && Finished == other.Finished;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((CategoryBllDto) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Count, InProgress, Finished);
        }
    }
}
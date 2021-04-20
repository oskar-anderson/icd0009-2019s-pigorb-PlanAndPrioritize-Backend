using System;
using System.Collections.Generic;
using ee.itcollege.pigorb.bookswap.Contracts.DAL.Base;

namespace BLL.App.DTO
{
    public class AppUserBllDto : IDomainBaseEntity<Guid>
    {
        public Guid Id { get; set; }
        
        public string FirstName { get; set; } = default!;
        
        public string LastName { get; set; } = default!;
        
        public string Email { get; set; } = default!;
        
        public ICollection<UserInVotingBllDto>? UserInVotings { get; set; }

        protected bool Equals(AppUserBllDto other)
        {
            return Id.Equals(other.Id) && FirstName == other.FirstName && LastName == other.LastName && Email == other.Email && Equals(UserInVotings, other.UserInVotings);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((AppUserBllDto) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, FirstName, LastName, Email, UserInVotings);
        }
    }
}
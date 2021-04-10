using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.App.DTO;
using Contracts.DAL.App.Repositories;

namespace Contracts.BLL.App.Services
{
    public interface IUserInVotingService : IUserInVotingRepository<UserInVotingBllDto>
    {
        void UpdateUsersInVoting(Guid votingId, ICollection<Guid> users);
    }
}
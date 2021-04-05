using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.App.DTO;
using Contracts.DAL.App.Repositories;

namespace Contracts.BLL.App.Services
{
    public interface IAppUserService : IAppUserRepository<AppUserBllDto>
    {
        Task<IEnumerable<AppUserBllDto>> GetUsersForVoting(Guid votingId);
    }
}
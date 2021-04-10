using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.App.DTO;
using BLL.App.DTO.Mappers;
using Contracts.BLL.App.Services;
using Contracts.DAL.App;
using Contracts.DAL.App.Repositories;
using DAL.App.DTO;
using ee.itcollege.pigorb.bookswap.BLL.Base.Services;

namespace BLL.App.Services
{
    public class AppUserService : BaseEntityService<IAppUserRepository, IAppUnitOfWork, AppUserDalDto, AppUserBllDto>, IAppUserService
    {
        private readonly BLLAppUserMapper _mapper = new BLLAppUserMapper();
        
        public AppUserService(IAppUnitOfWork unitOfWork)
            : base(unitOfWork, new BLLAppUserMapper(), unitOfWork.AppUsers)
        {
        }

        public async Task<IEnumerable<AppUserBllDto>> GetUsersForVoting(Guid votingId)
        {
            var users = await GetUsers();
            return users.ToList().FindAll(u => IsInVoting(u, votingId));
        }

        public async Task<IEnumerable<AppUserBllDto>> GetUsersNotInVoting(Guid votingId)
        {
            var users = await GetUsers();
            return users.ToList().FindAll(u => !IsInVoting(u, votingId));
        }

        private bool IsInVoting(AppUserBllDto user, Guid votingId)
        {
            return user.UserInVotings != null && user.UserInVotings.ToList().Any(u => u.VotingId == votingId);
        }

        public async Task<IEnumerable<AppUserBllDto>> GetUsers()
        {
            return (await ServiceRepository.GetUsers()).Select(dalEntity => _mapper.MapUser(dalEntity));
        }
    }
}
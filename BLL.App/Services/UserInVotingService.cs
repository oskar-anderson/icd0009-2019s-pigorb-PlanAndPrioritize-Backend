using System;
using System.Collections.Generic;
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
    public class UserInVotingService 
        : BaseEntityService<IUserInVotingRepository, IAppUnitOfWork, UserInVotingDalDto, UserInVotingBllDto>, IUserInVotingService
    {
        private readonly BLLUserInVotingMapper _mapper = new BLLUserInVotingMapper();
        
        public UserInVotingService(IAppUnitOfWork unitOfWork)
            : base(unitOfWork, new BLLUserInVotingMapper(), unitOfWork.UserInVotings)
        {
        }

        public async void UpdateUsersInVoting(Guid votingId, ICollection<Guid> users)
        {
            var existingUsers = await GetAllForVoting(votingId);
            foreach (var user in existingUsers)
            {
                if (!users.Contains(user.AppUserId))
                {
                    ServiceRepository.Remove(user);
                }
            }
            
            foreach (var userId in users)
            {
                if (await ServiceRepository.Exists(userId, votingId)) continue;
                var userInVoting = new UserInVotingDalDto
                {
                    VotingId = votingId,
                    AppUserId = userId
                };
                ServiceRepository.Add(userInVoting);
            }
        }

        public async Task<UserInVotingBllDto> FindUserInVoting(Guid userId, Guid votingId)
        {
            return _mapper.Map(await ServiceRepository.FindUserInVoting(userId, votingId));
        }

        public async Task<bool> Exists(Guid userId, Guid votingId)
        {
            return await ServiceRepository.Exists(userId, votingId);
        }

        public async Task<IEnumerable<UserInVotingDalDto>> GetAllForVoting(Guid votingId)
        {
            return await ServiceRepository.GetAllForVoting(votingId);
        }
    }
}
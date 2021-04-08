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

        public void AddUsersToVoting(Guid votingId, ICollection<Guid> users)
        {
            foreach (var userId in users)
            {
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
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTO.v1;
using BLL.App.DTO;
using BLL.App.DTO.Mappers;
using Contracts.BLL.App.Services;
using Contracts.DAL.App;
using Contracts.DAL.App.Repositories;
using DAL.App.DTO;
using ee.itcollege.pigorb.bookswap.BLL.Base.Services;

namespace BLL.App.Services
{
    public class UsersFeaturePriorityService 
        : BaseEntityService<IUsersFeaturePriorityRepository, IAppUnitOfWork, UsersFeaturePriorityDalDto, UsersFeaturePriorityBllDto>, 
            IUsersFeaturePriorityService
    {
        private readonly BLLUsersFeaturePriorityMapper _mapper = new BLLUsersFeaturePriorityMapper();
        
        public UsersFeaturePriorityService(IAppUnitOfWork unitOfWork)
            : base(unitOfWork, new BLLUsersFeaturePriorityMapper(), unitOfWork.UsersFeaturePriorities)
        {
        }

        public void AddUserPriorities(Dictionary<Guid, UsersFeaturePriorityCreateApiDto> featureInVotings, Guid userId)
        {
            foreach (var usersFeaturePriority in featureInVotings
                .Select(entry => new UsersFeaturePriorityDalDto
            {
                Size = entry.Value.TaskSize,
                BusinessValue = entry.Value.BusinessValue,
                TimeCriticality = entry.Value.TimeCriticality,
                RiskOrOpportunity = entry.Value.RiskOrOpportunity,
                PriorityValue = CalculatePriorityUsingWSJF(entry.Value.TaskSize, entry.Value.BusinessValue, 
                    entry.Value.TimeCriticality, entry.Value.RiskOrOpportunity),
                AppUserId = userId,
                FeatureInVotingId = entry.Key
            }))
            {
                ServiceRepository.Add(usersFeaturePriority);
            }
        }

        public async void UpdateUserPriorities(Dictionary<Guid, UsersFeaturePriorityCreateApiDto> featureInVotings, Guid userId)
        {
            foreach (var entry in featureInVotings)
            {
                var usersFeaturePriority = new UsersFeaturePriorityDalDto
                {
                    Size = entry.Value.TaskSize,
                    BusinessValue = entry.Value.BusinessValue,
                    TimeCriticality = entry.Value.TimeCriticality,
                    RiskOrOpportunity = entry.Value.RiskOrOpportunity,
                    PriorityValue = CalculatePriorityUsingWSJF(entry.Value.TaskSize, entry.Value.BusinessValue,
                        entry.Value.TimeCriticality, entry.Value.RiskOrOpportunity),
                    AppUserId = userId,
                    FeatureInVotingId = entry.Key
                };
                var existingPriority = await FirstOrDefaultForUserAndFeatureInVoting(userId, entry.Key);
                if (existingPriority == null)
                {
                    ServiceRepository.Add(usersFeaturePriority);
                }
                else
                {
                    usersFeaturePriority.Id = existingPriority.Id;
                    ServiceRepository.Update(usersFeaturePriority);
                }
            }
        }

        public async Task<IEnumerable<UsersFeaturePriorityBllDto>> GetAllForFeatureAndVoting(Guid featureId, Guid votingId)
        {
            return (await ServiceRepository.GetAllForFeatureAndVoting(featureId, votingId))
                .Select(dalEntity => _mapper.Map(dalEntity));
        }

        public async Task<IEnumerable<UsersFeaturePriorityBllDto>> GetPrioritiesForVotingAndUser(Guid votingId, Guid userId)
        {
            return (await ServiceRepository.GetPrioritiesForVotingAndUser(votingId, userId))
                .Select(dalEntity => _mapper.Map(dalEntity));
        }

        public async Task<bool> ExistsPrioritiesForVotingAndUser(Guid votingId, Guid userId)
        {
            return await ServiceRepository.ExistsPrioritiesForVotingAndUser(votingId, userId);
        }

        public async Task<UsersFeaturePriorityBllDto> FirstOrDefaultForUserAndFeatureInVoting(Guid userId, Guid featureInVotingId)
        {
            return _mapper.Map(await ServiceRepository.FirstOrDefaultForUserAndFeatureInVoting(userId, featureInVotingId));
        }
        
        public decimal CalculatePriorityUsingWSJF(int taskSize, int businessValue, int timeCriticality, int riskOrOpportunity)
        {
            var priceOfDelay = Convert.ToDecimal(businessValue) + Convert.ToDecimal(timeCriticality) +
                               Convert.ToDecimal(riskOrOpportunity);
            decimal wsjf;
            // UI won't allow 0 for size value but just to be sure
            if (taskSize == 0)
            {
                wsjf = priceOfDelay;
            }
            else
            {
                wsjf = priceOfDelay / Convert.ToDecimal(taskSize);
            }
            
            return decimal.Round(wsjf, 2);
        }
    }
}
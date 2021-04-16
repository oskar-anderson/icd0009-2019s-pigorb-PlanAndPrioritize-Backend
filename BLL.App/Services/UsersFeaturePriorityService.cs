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
            foreach (var usersFeaturePriority in featureInVotings.Select(entry => new UsersFeaturePriorityDalDto
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

        private decimal CalculatePriorityUsingWSJF(int taskSize, int businessValue, int timeCriticality, int riskOrOpportunity)
        {
            var priceOfDelay = Convert.ToDecimal(businessValue) + Convert.ToDecimal(timeCriticality) +
                               Convert.ToDecimal(riskOrOpportunity);
            var WSJF = priceOfDelay / Convert.ToDecimal(taskSize);
            return decimal.Round(WSJF, 2);
        }

        public async Task<IEnumerable<UsersFeaturePriorityBllDto>> GetAllForFeatureAndVoting(Guid featureId, Guid votingId)
        {
            return (await ServiceRepository.GetAllForFeatureAndVoting(featureId, votingId)).Select(dalEntity => _mapper.Map(dalEntity));
        }
    }
}
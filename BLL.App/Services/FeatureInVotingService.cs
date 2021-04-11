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
    public class FeatureInVotingService
        : BaseEntityService<IFeatureInVotingRepository, IAppUnitOfWork, FeatureInVotingDalDto, FeatureInVotingBllDto>,
            IFeatureInVotingService
    {
        private readonly BLLFeatureInVotingMapper _mapper = new BLLFeatureInVotingMapper();

        public FeatureInVotingService(IAppUnitOfWork unitOfWork)
            : base(unitOfWork, new BLLFeatureInVotingMapper(), unitOfWork.FeatureInVotings)
        {
        }

        public async void UpdateFeaturesInVoting(Guid votingId, ICollection<Guid> features)
        {
            var existingFeatures = await GetAllForVoting(votingId);
            foreach (var feature in existingFeatures)
            {
                if (!features.Contains(feature.FeatureId))
                {
                    ServiceRepository.Remove(feature);
                }
            }

            foreach (var featureId in features)
            {
                if (await ServiceRepository.Exists(featureId, votingId)) continue;
                var featureInVoting = new FeatureInVotingDalDto
                {
                    VotingId = votingId,
                    FeatureId = featureId
                };
                ServiceRepository.Add(featureInVoting);
            }
        }

        public async Task<FeatureInVotingBllDto> FindFeatureInVoting(Guid featureId, Guid votingId)
        {
            return _mapper.Map(await ServiceRepository.FindFeatureInVoting(featureId, votingId));
        }

        public async Task<bool> Exists(Guid featureId, Guid votingId)
        {
            return await ServiceRepository.Exists(featureId, votingId);
        }

        public async Task<IEnumerable<FeatureInVotingDalDto>> GetAllForVoting(Guid votingId)
        {
            return await ServiceRepository.GetAllForVoting(votingId);
        }

        public async Task<IEnumerable<FeatureInVotingDalDto>> GetAllForVotingWithUserPriorities(Guid votingId)
        {
            return await ServiceRepository.GetAllForVotingWithUserPriorities(votingId);
        }

        public async Task<Dictionary<Guid, UsersFeaturePriorityCreateApiDto>> GetFeatureInVotingIds(
            IEnumerable<UsersFeaturePriorityCreateApiDto> dtos)
        {
            Dictionary<Guid, UsersFeaturePriorityCreateApiDto> featureInVotings = new();
            foreach (var fp in dtos)
            {
                var featureInVoting = await ServiceRepository.FindFeatureInVoting(fp.Id, fp.VotingId);
                featureInVotings.Add(featureInVoting.Id, fp);
            }

            return featureInVotings;
        }

        public async Task CalculatePriorityValuesForVoting(Guid votingId)
        {
            var featuresForVoting = await ServiceRepository.GetAllForVotingWithUserPriorities(votingId);
            
            foreach (var fv in featuresForVoting)
            {
                var priorities = fv.UsersFeaturePriorities;
                if (priorities == null || priorities.Count == 0) continue;

                var averageSize = CalculateAverageSize(priorities);
                var averageBusinessValue = CalculateAverageBusinessValue(priorities);
                var averageTimeCriticality = CalculateAverageTimeCriticality(priorities);
                var averageRiskOrOpportunity = CalculateAverageRisk(priorities);

                var updatedFv = new FeatureInVotingDalDto
                {
                    Id = fv.Id,
                    VotingId = fv.VotingId,
                    FeatureId = fv.FeatureId,
                    AverageSize = averageSize,
                    AverageBusinessValue = averageBusinessValue,
                    AverageTimeCriticality = averageTimeCriticality,
                    AverageRiskOrOpportunity = averageRiskOrOpportunity,
                    AveragePriorityValue = CalculateAveragePriorityValueUsingWSJF(averageSize, averageBusinessValue,
                        averageTimeCriticality, averageRiskOrOpportunity)
                };
                ServiceRepository.Update(updatedFv);
            }
        }

        private decimal CalculateAveragePriorityValueUsingWSJF(decimal averageSize, decimal averageBusinessValue,
            decimal averageTimeCriticality, decimal averageRiskOrOpportunity)
        {
            var priceOfDelay = averageBusinessValue + averageTimeCriticality + averageRiskOrOpportunity;
            var WSJF = priceOfDelay / averageSize;
            return decimal.Round(WSJF, 2);
        }

        private decimal CalculateAverageSize(ICollection<UsersFeaturePriorityDalDto> userPriorities)
        {
            var sum = userPriorities.Sum(priority => priority.Size);
            return decimal.Round(Convert.ToDecimal(sum) / Convert.ToDecimal(userPriorities.Count), 2);
        }

        private decimal CalculateAverageBusinessValue(ICollection<UsersFeaturePriorityDalDto> userPriorities)
        {
            var sum = userPriorities.Sum(priority => priority.BusinessValue);
            return decimal.Round(Convert.ToDecimal(sum) / Convert.ToDecimal(userPriorities.Count), 2);
        }

        private decimal CalculateAverageTimeCriticality(ICollection<UsersFeaturePriorityDalDto> userPriorities)
        {
            var sum = userPriorities.Sum(priority => priority.TimeCriticality);
            return decimal.Round(Convert.ToDecimal(sum) / Convert.ToDecimal(userPriorities.Count), 2);
        }

        private decimal CalculateAverageRisk(ICollection<UsersFeaturePriorityDalDto> userPriorities)
        {
            var sum = userPriorities.Sum(priority => priority.RiskOrOpportunity);
            return decimal.Round(Convert.ToDecimal(sum) / Convert.ToDecimal(userPriorities.Count), 2);
        }
    }
}
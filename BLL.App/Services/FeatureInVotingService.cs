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
    }
}
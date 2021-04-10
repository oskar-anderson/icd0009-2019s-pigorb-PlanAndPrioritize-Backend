using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTO.v1;
using BLL.App.DTO;
using Contracts.DAL.App.Repositories;

namespace Contracts.BLL.App.Services
{
    public interface IFeatureService : IFeatureRepository<FeatureBllDto>
    {
        FeatureBllDto AddWithMetaData(FeatureBllDto feature, Guid userId);

        ICollection<FeatureStatusApiDto> GetFeatureStatuses();

        FeatureBllDto ConstructEditedFeatureWithChangeLog(FeatureBllDto bllFeature, FeatureEditApiDto featureDto, 
            string userName, CategoryBllDto category, AppUserBllDto? assignee);
        
        Task<IEnumerable<FeatureBllDto>> GetFeaturesForVoting(Guid votingId);
        
        Task<IEnumerable<FeatureBllDto>> GetToDoFeaturesNotInVoting(Guid votingId);
    }
}
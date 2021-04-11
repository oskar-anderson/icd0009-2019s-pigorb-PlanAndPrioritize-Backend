using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTO.v1;
using BLL.App.DTO;
using Contracts.DAL.App.Repositories;

namespace Contracts.BLL.App.Services
{
    public interface IFeatureInVotingService : IFeatureInVotingRepository<FeatureInVotingBllDto>
    {
        void UpdateFeaturesInVoting(Guid votingId, ICollection<Guid> features);
        Task<Dictionary<Guid, UsersFeaturePriorityCreateApiDto>> GetFeatureInVotingIds(IEnumerable<UsersFeaturePriorityCreateApiDto> dtos);
        Task CalculatePriorityValuesForVoting(Guid votingId);
    }
}
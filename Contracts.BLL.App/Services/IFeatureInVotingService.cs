using System;
using System.Collections.Generic;
using BLL.App.DTO;
using Contracts.DAL.App.Repositories;

namespace Contracts.BLL.App.Services
{
    public interface IFeatureInVotingService : IFeatureInVotingRepository<FeatureInVotingBllDto>
    {
        void UpdateFeaturesInVoting(Guid votingId, ICollection<Guid> features);
    }
}
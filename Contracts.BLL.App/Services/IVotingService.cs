using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.App.DTO;
using Contracts.DAL.App.Repositories;

namespace Contracts.BLL.App.Services
{
    public interface IVotingService : IVotingRepository<VotingBllDto>
    {
        Task<IEnumerable<VotingBllDto>> GetVotingsForFeature(Guid id);
        
        Task<IEnumerable<VotingBllDto>> GetActiveVotingsNotInFeature(Guid featureId);
        Task<VotingEditBllDto> GetVotingWithIdCollections(Guid id);
        
        Task<IEnumerable<VotingBllDto>> GetAssignedVotings(Guid userId);
    }
}
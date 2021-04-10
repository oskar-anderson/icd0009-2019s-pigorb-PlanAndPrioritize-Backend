using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.App.DTO;
using ee.itcollege.pigorb.bookswap.Contracts.DAL.Base;
using ee.itcollege.pigorb.bookswap.Contracts.DAL.Base.Repositories;

namespace Contracts.DAL.App.Repositories
{
    public interface IFeatureInVotingRepository : IFeatureInVotingRepository<FeatureInVotingDalDto>
    {
    }
    
    public interface IFeatureInVotingRepository<TDALEntity> : IBaseRepository<TDALEntity>
        where TDALEntity : class, IDomainBaseEntity<Guid>, new()
    {
        Task<TDALEntity> FindFeatureInVoting(Guid featureId, Guid votingId);
        
        Task<bool> Exists(Guid featureId, Guid votingId);
        
        Task<IEnumerable<FeatureInVotingDalDto>> GetAllForVoting(Guid votingId);
    }
}
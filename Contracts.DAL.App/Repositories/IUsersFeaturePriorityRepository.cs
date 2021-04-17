using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.App.DTO;
using ee.itcollege.pigorb.bookswap.Contracts.DAL.Base;
using ee.itcollege.pigorb.bookswap.Contracts.DAL.Base.Repositories;

namespace Contracts.DAL.App.Repositories
{
    public interface IUsersFeaturePriorityRepository : IUsersFeaturePriorityRepository<UsersFeaturePriorityDalDto>
    {
    }
    
    public interface IUsersFeaturePriorityRepository<TDALEntity> : IBaseRepository<TDALEntity>
        where TDALEntity : class, IDomainBaseEntity<Guid>, new()
    {
        Task<IEnumerable<TDALEntity>> GetAllForFeatureAndVoting(Guid featureId, Guid votingId);
        Task<IEnumerable<TDALEntity>> GetPrioritiesForVotingAndUser(Guid votingId, Guid userId);
        Task<bool> ExistsPrioritiesForVotingAndUser(Guid votingId, Guid userId);
        
        Task<TDALEntity> FirstOrDefaultForUserAndFeatureInVoting(Guid userId, Guid featureInVotingId);
    }
}
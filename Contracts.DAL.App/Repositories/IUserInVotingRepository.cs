using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.App.DTO;
using ee.itcollege.pigorb.bookswap.Contracts.DAL.Base;
using ee.itcollege.pigorb.bookswap.Contracts.DAL.Base.Repositories;

namespace Contracts.DAL.App.Repositories
{
    public interface IUserInVotingRepository : IUserInVotingRepository<UserInVotingDalDto>
    {
    }
    
    public interface IUserInVotingRepository<TDALEntity> : IBaseRepository<TDALEntity>
        where TDALEntity : class, IDomainBaseEntity<Guid>, new()
    {
        Task<TDALEntity> FindUserInVoting(Guid userId, Guid votingId);
        
        Task<bool> Exists(Guid userId, Guid votingId);
        
        Task<IEnumerable<UserInVotingDalDto>> GetAllForVoting(Guid votingId);
    }
}
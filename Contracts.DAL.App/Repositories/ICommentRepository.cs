using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.App.DTO;
using ee.itcollege.pigorb.bookswap.Contracts.DAL.Base;
using ee.itcollege.pigorb.bookswap.Contracts.DAL.Base.Repositories;

namespace Contracts.DAL.App.Repositories
{
    public interface ICommentRepository : ICommentRepository<CommentDalDto>
    {
    }
    
    public interface ICommentRepository<TDALEntity> : IBaseRepository<TDALEntity>
        where TDALEntity : class, IDomainBaseEntity<Guid>, new()
    {
        Task<IEnumerable<TDALEntity>> GetCommentsForFeature(Guid featureId);
    }
}
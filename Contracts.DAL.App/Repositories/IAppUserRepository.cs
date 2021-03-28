using System;
using System.Threading.Tasks;
using DAL.App.DTO;
using ee.itcollege.pigorb.bookswap.Contracts.DAL.Base;
using ee.itcollege.pigorb.bookswap.Contracts.DAL.Base.Repositories;

namespace Contracts.DAL.App.Repositories
{
    public interface IAppUserRepository : IAppUserRepository<AppUserDalDto>
    {
    }
    
    public interface IAppUserRepository<TDALEntity> : IBaseRepository<TDALEntity>
        where TDALEntity : class, IDomainBaseEntity<Guid>, new()
    {
    }
}
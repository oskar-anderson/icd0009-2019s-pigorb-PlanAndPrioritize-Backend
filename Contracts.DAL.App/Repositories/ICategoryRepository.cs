using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.App.DTO;
using ee.itcollege.pigorb.bookswap.Contracts.DAL.Base;
using ee.itcollege.pigorb.bookswap.Contracts.DAL.Base.Repositories;

namespace Contracts.DAL.App.Repositories
{
    public interface ICategoryRepository : ICategoryRepository<CategoryDalDto>
    {
    }
    
    public interface ICategoryRepository<TDALEntity> : IBaseRepository<TDALEntity>
        where TDALEntity : class, IDomainBaseEntity<Guid>, new()
    {
        Task<IEnumerable<TDALEntity>> GetAll();
        Task<IEnumerable<TDALEntity>> GetAllPlain();
        Task<bool> Exists(Guid id);
        Task<TDALEntity> FirstOrDefault(Guid id);
        Task Delete(Guid id);
        TDALEntity Edit(TDALEntity entity);
    }
}
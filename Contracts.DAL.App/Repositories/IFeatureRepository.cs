using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.App.DTO;
using ee.itcollege.pigorb.bookswap.Contracts.DAL.Base;
using ee.itcollege.pigorb.bookswap.Contracts.DAL.Base.Repositories;

namespace Contracts.DAL.App.Repositories
{
    public interface IFeatureRepository : IFeatureRepository<FeatureDalDto>
    {
    }
    
    public interface IFeatureRepository<TDALEntity> : IBaseRepository<TDALEntity>
        where TDALEntity : class, IDomainBaseEntity<Guid>, new()
    {
        Task<IEnumerable<TDALEntity>> GetAll();
        IEnumerable<TDALEntity> GetAllWithVotings(string? search, int limit);
        Task<IEnumerable<TDALEntity>> GetFeaturesForGraph(int limit);
        Task<IEnumerable<TDALEntity>> GetToDoFeatures();
        Task<bool> Exists(Guid id);
        Task<TDALEntity> FirstOrDefault(Guid id);
        Task<TDALEntity> GetFeaturePlain(Guid id);
        Task<TDALEntity> GetLatestFeature();
        Task Delete(Guid id);
        TDALEntity Edit(TDALEntity entity);
    }
}
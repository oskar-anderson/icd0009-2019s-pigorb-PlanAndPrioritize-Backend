using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.DAL.App.Repositories;
using DAL.App.DTO;
using DAL.App.DTO.Mappers;
using Domain;
using ee.itcollege.pigorb.bookswap.DAL.Base.EF.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DAL.App.EF.Repositories
{
    public class FeatureRepository : EFBaseRepository<AppDbContext, Feature, FeatureDalDto>, IFeatureRepository
    {
        private readonly DALFeatureMapper _mapper = new ();
        
        public FeatureRepository(AppDbContext dbContext) : base(dbContext, new DALFeatureMapper())
        {
        }
        
        public async Task<IEnumerable<FeatureDalDto>> GetAll()
        {
            var features = RepoDbContext.Features
                .Include(f => f.Category)
                .Include(f => f.AppUser)
                .Include(f => f.Comments)
                    .ThenInclude(c => c.AppUser)
                .Include(f => f.FeatureInVotings)
                    .ThenInclude(fv => fv.Voting)
                .Select(dbEntity => _mapper.MapFeature(dbEntity))
                .AsNoTracking();
            return await features.ToListAsync();
        }

        public async Task<IEnumerable<FeatureDalDto>> GetAllWithoutCollections()
        {
            var features = RepoDbContext.Features
                .Include(f => f.Category)
                .Include(f => f.AppUser)
                .Select(dbEntity => _mapper.MapFeature(dbEntity))
                .AsNoTracking();
            return await features.ToListAsync();
        }
        
        public async Task<bool> Exists(Guid id)
        {
            return await RepoDbSet.AnyAsync(a => a.Id == id);
        }

        public async Task<FeatureDalDto> FirstOrDefault(Guid id)
        {
            var query = RepoDbSet
                .Include(f => f.Category)
                .Include(f => f.AppUser)
                .Include(f => f.Comments)
                    .ThenInclude(c => c.AppUser)
                .Include(f => f.FeatureInVotings)
                    .ThenInclude(fv => fv.Voting)
                .Where(a => a.Id == id).AsQueryable();
            
            return _mapper.MapFeature(await query.AsNoTracking().FirstOrDefaultAsync());
        }

        public async Task Delete(Guid id)
        {
            var query = RepoDbSet.Where(a => a.Id == id).AsQueryable();
            
            var feature = await query.AsNoTracking().FirstOrDefaultAsync();
            base.Remove(feature.Id);
        }

        public FeatureDalDto Edit(FeatureDalDto dalEntity)
        {
            var entity = Mapper.Map(dalEntity);
            
            var trackedEntity = RepoDbSet.Update(entity).Entity;
            var result = Mapper.Map(trackedEntity);
            return result;
        }
    }
}

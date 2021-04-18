using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Classifiers;
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
        private readonly AppDbContext _dbContext;
        private readonly DALFeatureMapper _mapper = new ();
        
        public FeatureRepository(AppDbContext dbContext) : base(dbContext, new DALFeatureMapper())
        {
            _dbContext = dbContext;
        }
        
        public async Task<IEnumerable<FeatureDalDto>> GetAll()
        {
            var features = RepoDbSet
                .Include(f => f.Category)
                .Include(f => f.AppUser)
                .Include(f => f.CreatedBy)
                .Include(f => f.Comments)
                    .ThenInclude(c => c.AppUser)
                .Include(f => f.FeatureInVotings)
                    .ThenInclude(fv => fv.Voting)
                .OrderByDescending(f => f.TimeCreated)
                .Select(dbEntity => _mapper.MapFeature(dbEntity))
                .AsNoTracking();
            return await features.ToListAsync();
        }
        
        public IEnumerable<FeatureDalDto> GetAllWithVotings(string? search)
        {
            var featuresQuery = RepoDbSet
                .Include(f => f.Category)
                .Include(f => f.AppUser)
                .Include(f => f.FeatureInVotings)
                    .ThenInclude(fv => fv.Voting)
                .Select(dbEntity => _mapper.MapFeature(dbEntity))
                .AsNoTracking()
                .AsQueryable();
            
            var newQuery = featuresQuery
                .AsEnumerable()
                .Where(f => ContainsSearch(f, search));
            
            newQuery = newQuery.OrderByDescending(f => f.TimeCreated);
            
            return newQuery;
        }
        
        public async Task<IEnumerable<FeatureDalDto>> GetFeaturesForGraph()
        {
            var features = RepoDbSet
                .Include(f => f.Category)
                .Where(f => f.StartTime != null && f.EndTime != null)
                .OrderByDescending(f => f.TimeCreated)
                .Select(dbEntity => _mapper.Map(dbEntity))
                .AsNoTracking();
            return await features.ToListAsync();
        }

        private bool ContainsSearch(FeatureDalDto feature, string? search)
        {
            if (string.IsNullOrEmpty(search))
            {
                return true;
            }
            search = search.ToLower();
            return feature.Title.ToLower().Contains(search) || 
                   feature.Description == null || 
                   feature.Description.ToLower().Contains(search);
        }

        public async Task<IEnumerable<FeatureDalDto>> GetToDoFeatures()
        {
            var features = RepoDbSet
                .Where(f => f.FeatureStatus == FeatureStatus.NotStarted)
                .Select(dbEntity => _mapper.Map(dbEntity))
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
                .Include(f => f.CreatedBy)
                .Include(f => f.Comments)
                    .ThenInclude(c => c.AppUser)
                .Include(f => f.FeatureInVotings)
                    .ThenInclude(fv => fv.Voting)
                .Where(a => a.Id == id).AsQueryable();
            
            return _mapper.MapFeature(await query.AsNoTracking().FirstOrDefaultAsync());
        }

        public async Task<FeatureDalDto> GetFeaturePlain(Guid id)
        {
            var query = RepoDbSet.Where(a => a.Id == id).AsQueryable();
            
            return _mapper.Map(await query.AsNoTracking().FirstOrDefaultAsync());
        }

        public async Task<FeatureDalDto> GetLatestFeature()
        {
            var query = RepoDbSet
                .Include(f => f.Category)
                .Include(f => f.AppUser)
                .Include(f => f.CreatedBy)
                .Include(f => f.Comments)
                .ThenInclude(c => c.AppUser)
                .Include(f => f.FeatureInVotings)
                .ThenInclude(fv => fv.Voting)
                .OrderByDescending(f => f.TimeCreated);
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
            // Added manually because on edit feature is not marked as modified and won't be updated. Why?
            _dbContext.Entry(entity).Property(f => f.Title).IsModified = true;
            
            var trackedEntity = RepoDbSet.Update(entity).Entity;
            var result = Mapper.Map(trackedEntity);
            return result;
        }
    }
}

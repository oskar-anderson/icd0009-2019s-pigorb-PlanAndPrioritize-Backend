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
    public class FeatureInVotingRepository : EFBaseRepository<AppDbContext, FeatureInVoting, FeatureInVotingDalDto>, IFeatureInVotingRepository
    {
        private readonly DALFeatureInVotingMapper _mapper = new DALFeatureInVotingMapper();
        
        public FeatureInVotingRepository(AppDbContext dbContext) : base(dbContext, new DALFeatureInVotingMapper())
        {
        }

        public async Task<FeatureInVotingDalDto> FindFeatureInVoting(Guid featureId, Guid votingId)
        {
            var query = RepoDbSet
                .Where(f => f.FeatureId == featureId && f.VotingId == votingId)
                .AsQueryable();
            return Mapper.Map(await query.AsNoTracking().FirstOrDefaultAsync());
        }

        public async Task<bool> Exists(Guid featureId, Guid votingId)
        {
            return await RepoDbSet.AnyAsync(fv => fv.FeatureId == featureId && fv.VotingId == votingId);
        }

        public async Task<IEnumerable<FeatureInVotingDalDto>> GetAllForVoting(Guid votingId)
        {
            var featureInVotings = RepoDbSet
                .Where(fv => fv.VotingId == votingId)
                .Select(dbEntity => _mapper.Map(dbEntity))
                .AsNoTracking();
            return await featureInVotings.ToListAsync();
        }
        
        public async Task<IEnumerable<FeatureInVotingDalDto>> GetAllForVotingWithUserPriorities(Guid votingId)
        {
            var featureInVotings = RepoDbSet
                .Where(fv => fv.VotingId == votingId)
                .Include(fv => fv.UsersFeaturePriorities)
                .Select(dbEntity => _mapper.Map(dbEntity))
                .AsNoTracking();
            return await featureInVotings.ToListAsync();
        }
        
        
    }
}

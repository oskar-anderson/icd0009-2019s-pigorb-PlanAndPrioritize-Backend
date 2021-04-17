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
    public class UsersFeaturePriorityRepository 
        : EFBaseRepository<AppDbContext, UsersFeaturePriority, UsersFeaturePriorityDalDto>, IUsersFeaturePriorityRepository
    {
        private readonly DALUsersFeaturePriorityMapper _mapper = new DALUsersFeaturePriorityMapper();
        
        public UsersFeaturePriorityRepository(AppDbContext dbContext) : base(dbContext, new DALUsersFeaturePriorityMapper())
        {
        }
        
        public async Task<IEnumerable<UsersFeaturePriorityDalDto>> GetAllForFeatureAndVoting(Guid featureId, Guid votingId)
        {
            var priorities = RepoDbSet
                .Include(u => u.AppUser)
                .Include(u => u.FeatureInVoting)
                .Where(u => u.FeatureInVoting != null && u.FeatureInVoting.FeatureId == featureId && u.FeatureInVoting.VotingId == votingId)
                .OrderBy(u => u.AppUser!.LastName)
                .Select(dbEntity => _mapper.MapUserPriority(dbEntity))
                .AsNoTracking();
            return await priorities.ToListAsync();
        }

        public async Task<IEnumerable<UsersFeaturePriorityDalDto>> GetPrioritiesForVotingAndUser(Guid votingId, Guid userId)
        {
            var priorities = RepoDbSet
                .Include(u => u.AppUser)
                .Include(u => u.FeatureInVoting)
                    .ThenInclude(f => f!.Feature)
                        .ThenInclude(f => f!.Category)
                .Where(u => u.FeatureInVoting != null && 
                            u.FeatureInVoting.VotingId == votingId && 
                            u.AppUserId == userId)
                .OrderBy(u => u.AppUser!.LastName)
                .Select(dbEntity => _mapper.MapUserPriorityWithSubData(dbEntity))
                .AsNoTracking();
            return await priorities.ToListAsync();
        }
        
        public async Task<bool> ExistsPrioritiesForVotingAndUser(Guid votingId, Guid userId)
        {
            var priorities = RepoDbSet
                .Include(u => u.FeatureInVoting)
                .Where(u => u.FeatureInVoting != null && 
                            u.FeatureInVoting.VotingId == votingId && 
                            u.AppUserId == userId)
                .AsNoTracking()
                .ToListAsync();
            return (await priorities).Count > 0;
        }

        public async Task<UsersFeaturePriorityDalDto> FirstOrDefaultForUserAndFeatureInVoting(Guid userId, Guid featureInVotingId)
        {
            var query = RepoDbSet
                .Where(u => u.AppUserId == userId && u.FeatureInVotingId == featureInVotingId)
                .AsQueryable();
            return _mapper.Map(await query.AsNoTracking().FirstOrDefaultAsync());
        }
    }
}

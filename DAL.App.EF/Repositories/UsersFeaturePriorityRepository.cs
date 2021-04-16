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
      }
}

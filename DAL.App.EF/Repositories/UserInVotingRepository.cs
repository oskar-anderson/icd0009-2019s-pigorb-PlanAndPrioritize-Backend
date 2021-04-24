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
    public class UserInVotingRepository : EFBaseRepository<AppDbContext, UserInVoting, UserInVotingDalDto>,
        IUserInVotingRepository
    {
        private readonly DALUserInVotingMapper _mapper = new DALUserInVotingMapper();

        public UserInVotingRepository(AppDbContext dbContext) : base(dbContext, new DALUserInVotingMapper())
        {
        }

        public async Task<UserInVotingDalDto> FindUserInVoting(Guid userId, Guid votingId)
        {
            var query = RepoDbSet
                .Where(f => f.AppUserId == userId && f.VotingId == votingId)
                .AsQueryable();
            return Mapper.Map(await query.AsNoTracking().FirstOrDefaultAsync());
        }

        public async Task<bool> Exists(Guid userId, Guid votingId)
        {
            return await RepoDbSet.AnyAsync(uv => uv.AppUserId == userId && uv.VotingId == votingId);
        }

        public async Task<IEnumerable<UserInVotingDalDto>> GetAllForVoting(Guid votingId)
        {
            var userInVotings = RepoDbSet
                .Where(uv => uv.VotingId == votingId)
                .Select(dbEntity => _mapper.Map(dbEntity))
                .AsNoTracking();
            return await userInVotings.ToListAsync();
        }

        public async Task<bool> HasAssignedOpenVotings(Guid userId)
        {
            return await RepoDbSet
                .Include(uv => uv.Voting)
                .AnyAsync(uv => uv.AppUserId == userId &&
                                uv.Voting!.StartTime < DateTime.Now
                                && uv.Voting!.StartTime < DateTime.Now &&
                                uv.Voting!.EndTime > DateTime.Now);
        }
    }
}
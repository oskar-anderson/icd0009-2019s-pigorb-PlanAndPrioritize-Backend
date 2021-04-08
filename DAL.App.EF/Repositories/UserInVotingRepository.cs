using System;
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
    public class UserInVotingRepository : EFBaseRepository<AppDbContext, UserInVoting, UserInVotingDalDto>, IUserInVotingRepository
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
    }
}

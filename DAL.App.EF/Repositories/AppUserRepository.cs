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
    public class AppUserRepository : EFBaseRepository<AppDbContext, AppUser, AppUserDalDto>, IAppUserRepository
    {
        private readonly DALAppUserMapper _mapper = new DALAppUserMapper();

        public AppUserRepository(AppDbContext dbContext) : base(dbContext, new DALAppUserMapper())
        {
        }

        public async Task<IEnumerable<AppUserDalDto>> GetUsers()
        {
            var users = RepoDbContext.AppUsers
                .Include(u => u.UserInVotings)
                    .ThenInclude(uv => uv.Voting)
                .Select(dbEntity => _mapper.MapUser(dbEntity))
                .AsNoTracking();
            return await users.ToListAsync();
        }
    }
}

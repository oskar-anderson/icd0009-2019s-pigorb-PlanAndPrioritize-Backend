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
    public class VotingRepository : EFBaseRepository<AppDbContext, Voting, VotingDalDto>, IVotingRepository
    {
        private readonly DALVotingMapper _mapper = new DALVotingMapper();
        
        public VotingRepository(AppDbContext dbContext) : base(dbContext, new DALVotingMapper())
        {
        }

        public async Task<IEnumerable<VotingDalDto>> GetAll()
        {
            var votings = RepoDbSet
                .Include(v => v.FeatureInVotings)
                    .ThenInclude(v => v.Feature)
                .Include(v => v.UserInVotings)
                    .ThenInclude(uv => uv.AppUser)
                .Select(dbEntity => _mapper.MapVoting(dbEntity))
                .AsNoTracking();
            return await votings.ToListAsync();
        }

        public async Task<IEnumerable<VotingDalDto>> GetActiveVotings()
        {
            var votings = RepoDbSet
                .Where(v => v.EndTime > DateTime.Now)
                .Select(dbEntity => _mapper.Map(dbEntity))
                .AsNoTracking();
            return await votings.ToListAsync();
        }

        public async Task<IEnumerable<VotingDalDto>> GetActiveVotingsWithCollections()
        {
            var votings = RepoDbSet
                .Where(v => v.EndTime > DateTime.Now)
                .Include(v => v.FeatureInVotings)
                    .ThenInclude(v => v.Feature)
                .Include(v => v.UserInVotings)
                    .ThenInclude(uv => uv.AppUser)
                .Select(dbEntity => _mapper.MapVoting(dbEntity))
                .AsNoTracking();
            return await votings.ToListAsync();
        }

        public async Task<IEnumerable<VotingDalDto>> GetAllPlain()
        {
            var votings = RepoDbSet
                .Select(dbEntity => _mapper.Map(dbEntity))
                .AsNoTracking();
            return await votings.ToListAsync();
        }
        
        public async Task<bool> Exists(Guid id)
        {
            return await RepoDbSet.AnyAsync(a => a.Id == id);
        }

        public async Task<VotingDalDto> FirstOrDefault(Guid id)
        {
            var query = RepoDbSet
                .Include(v => v.FeatureInVotings)
                    .ThenInclude(fv => fv.Feature)
                .Include(v => v.UserInVotings)
                    .ThenInclude(uv => uv.AppUser)
                .Where(a => a.Id == id)
                .AsQueryable();
            return _mapper.MapVoting(await query.AsNoTracking().FirstOrDefaultAsync());
        }

        public async Task Delete(Guid id)
        {
            var query = RepoDbSet.Where(a => a.Id == id).AsQueryable();
            
            var voting = await query.AsNoTracking().FirstOrDefaultAsync();
            base.Remove(voting.Id);
        }

        public VotingDalDto Edit(VotingDalDto dalEntity)
        {
            var entity = Mapper.Map(dalEntity);
            
            var trackedEntity = RepoDbSet.Update(entity).Entity;
            var result = Mapper.Map(trackedEntity);
            return result;
        }
    }
}

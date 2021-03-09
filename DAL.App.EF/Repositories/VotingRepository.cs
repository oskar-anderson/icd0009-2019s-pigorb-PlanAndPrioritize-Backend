using Contracts.DAL.App.Repositories;
using DAL.App.DTO;
using DAL.App.DTO.Mappers;
using Domain;
using ee.itcollege.pigorb.bookswap.DAL.Base.EF.Repositories;

namespace DAL.App.EF.Repositories
{
    public class VotingRepository : EFBaseRepository<AppDbContext, Voting, VotingDalDto>, IVotingRepository
    {
        private readonly DALVotingMapper _mapper = new DALVotingMapper();
        
        public VotingRepository(AppDbContext dbContext) : base(dbContext, new DALVotingMapper())
        {
        }
        
      }
}

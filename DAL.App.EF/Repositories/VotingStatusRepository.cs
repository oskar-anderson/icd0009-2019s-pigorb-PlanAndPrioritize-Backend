using Contracts.DAL.App.Repositories;
using DAL.App.DTO;
using DAL.App.DTO.Mappers;
using Domain;
using ee.itcollege.pigorb.bookswap.DAL.Base.EF.Repositories;

namespace DAL.App.EF.Repositories
{
    public class VotingStatusRepository : EFBaseRepository<AppDbContext, VotingStatus, VotingStatusDalDto>, IVotingStatusRepository
    {
        private readonly DALVotingStatusMapper _mapper = new DALVotingStatusMapper();
        
        public VotingStatusRepository(AppDbContext dbContext) : base(dbContext, new DALVotingStatusMapper())
        {
        }
        
      }
}

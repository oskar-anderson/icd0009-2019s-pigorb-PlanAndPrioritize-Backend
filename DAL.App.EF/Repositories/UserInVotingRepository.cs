using Contracts.DAL.App.Repositories;
using DAL.App.DTO;
using DAL.App.DTO.Mappers;
using Domain;
using ee.itcollege.pigorb.bookswap.DAL.Base.EF.Repositories;

namespace DAL.App.EF.Repositories
{
    public class UserInVotingRepository : EFBaseRepository<AppDbContext, UserInVoting, UserInVotingDalDto>, IUserInVotingRepository
    {
        private readonly DALUserInVotingMapper _mapper = new DALUserInVotingMapper();
        
        public UserInVotingRepository(AppDbContext dbContext) : base(dbContext, new DALUserInVotingMapper())
        {
        }
        
      }
}

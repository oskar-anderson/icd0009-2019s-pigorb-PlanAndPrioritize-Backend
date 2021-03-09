using Contracts.DAL.App.Repositories;
using DAL.App.DTO;
using DAL.App.DTO.Mappers;
using Domain;
using ee.itcollege.pigorb.bookswap.DAL.Base.EF.Repositories;

namespace DAL.App.EF.Repositories
{
    public class FeatureInVotingRepository : EFBaseRepository<AppDbContext, FeatureInVoting, FeatureInVotingDalDto>, IFeatureInVotingRepository
    {
        private readonly DALFeatureInVotingMapper _mapper = new DALFeatureInVotingMapper();
        
        public FeatureInVotingRepository(AppDbContext dbContext) : base(dbContext, new DALFeatureInVotingMapper())
        {
        }
        
      }
}

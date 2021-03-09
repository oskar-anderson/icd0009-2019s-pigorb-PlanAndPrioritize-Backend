using Contracts.DAL.App.Repositories;
using DAL.App.DTO;
using DAL.App.DTO.Mappers;
using Domain;
using ee.itcollege.pigorb.bookswap.DAL.Base.EF.Repositories;

namespace DAL.App.EF.Repositories
{
    public class FeatureStatusRepository : EFBaseRepository<AppDbContext, FeatureStatus, FeatureStatusDalDto>, IFeatureStatusRepository
    {
        private readonly DALFeatureStatusMapper _mapper = new DALFeatureStatusMapper();
        
        public FeatureStatusRepository(AppDbContext dbContext) : base(dbContext, new DALFeatureStatusMapper())
        {
        }
        
      }
}

using Contracts.DAL.App.Repositories;
using DAL.App.DTO;
using DAL.App.DTO.Mappers;
using Domain;
using ee.itcollege.pigorb.bookswap.DAL.Base.EF.Repositories;

namespace DAL.App.EF.Repositories
{
    public class FeatureRepository : EFBaseRepository<AppDbContext, Feature, FeatureDalDto>, IFeatureRepository
    {
        private readonly DALFeatureMapper _mapper = new DALFeatureMapper();
        
        public FeatureRepository(AppDbContext dbContext) : base(dbContext, new DALFeatureMapper())
        {
        }
        
      }
}

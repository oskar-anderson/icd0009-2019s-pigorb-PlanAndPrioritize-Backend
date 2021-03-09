using BLL.App.DTO;
using BLL.App.DTO.Mappers;
using Contracts.BLL.App.Services;
using Contracts.DAL.App;
using Contracts.DAL.App.Repositories;
using DAL.App.DTO;
using ee.itcollege.pigorb.bookswap.BLL.Base.Services;

namespace BLL.App.Services
{
    public class FeatureService : BaseEntityService<IFeatureRepository, IAppUnitOfWork, FeatureDalDto, FeatureBllDto>, IFeatureService
    {
        private readonly BLLFeatureMapper _mapper = new BLLFeatureMapper();
        
        public FeatureService(IAppUnitOfWork unitOfWork)
            : base(unitOfWork, new BLLFeatureMapper(), unitOfWork.Features)
        {
        }
        
    }
}
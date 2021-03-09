using BLL.App.DTO;
using BLL.App.DTO.Mappers;
using Contracts.BLL.App.Services;
using Contracts.DAL.App;
using Contracts.DAL.App.Repositories;
using DAL.App.DTO;
using ee.itcollege.pigorb.bookswap.BLL.Base.Services;

namespace BLL.App.Services
{
    public class FeatureStatusService 
        : BaseEntityService<IFeatureStatusRepository, IAppUnitOfWork, FeatureStatusDalDto, FeatureStatusBllDto>, IFeatureStatusService
    {
        private readonly BLLFeatureStatusMapper _mapper = new BLLFeatureStatusMapper();
        
        public FeatureStatusService(IAppUnitOfWork unitOfWork)
            : base(unitOfWork, new BLLFeatureStatusMapper(), unitOfWork.FeatureStatuses)
        {
        }
        
    }
}
using BLL.App.DTO;
using BLL.App.DTO.Mappers;
using Contracts.BLL.App.Services;
using Contracts.DAL.App;
using Contracts.DAL.App.Repositories;
using DAL.App.DTO;
using ee.itcollege.pigorb.bookswap.BLL.Base.Services;

namespace BLL.App.Services
{
    public class PriorityStatusService 
        : BaseEntityService<IPriorityStatusRepository, IAppUnitOfWork, PriorityStatusDalDto, PriorityStatusBllDto>, 
            IPriorityStatusService
    {
        private readonly BLLPriorityStatusMapper _mapper = new BLLPriorityStatusMapper();
        
        public PriorityStatusService(IAppUnitOfWork unitOfWork)
            : base(unitOfWork, new BLLPriorityStatusMapper(), unitOfWork.PriorityStatuses)
        {
        }
        
    }
}
using BLL.App.DTO;
using BLL.App.DTO.Mappers;
using Contracts.BLL.App.Services;
using Contracts.DAL.App;
using Contracts.DAL.App.Repositories;
using DAL.App.DTO;
using ee.itcollege.pigorb.bookswap.BLL.Base.Services;

namespace BLL.App.Services
{
    public class VotingStatusService 
        : BaseEntityService<IVotingStatusRepository, IAppUnitOfWork, VotingStatusDalDto, VotingStatusBllDto>, 
            IVotingStatusService
    {
        private readonly BLLVotingStatusMapper _mapper = new BLLVotingStatusMapper();
        
        public VotingStatusService(IAppUnitOfWork unitOfWork)
            : base(unitOfWork, new BLLVotingStatusMapper(), unitOfWork.VotingStatuses)
        {
        }
        
    }
}
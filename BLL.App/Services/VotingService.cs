using BLL.App.DTO;
using BLL.App.DTO.Mappers;
using Contracts.BLL.App.Services;
using Contracts.DAL.App;
using Contracts.DAL.App.Repositories;
using DAL.App.DTO;
using ee.itcollege.pigorb.bookswap.BLL.Base.Services;

namespace BLL.App.Services
{
    public class VotingService : BaseEntityService<IVotingRepository, IAppUnitOfWork, VotingDalDto, VotingBllDto>, IVotingService
    {
        private readonly BLLVotingMapper _mapper = new BLLVotingMapper();
        
        public VotingService(IAppUnitOfWork unitOfWork)
            : base(unitOfWork, new BLLVotingMapper(), unitOfWork.Votings)
        {
        }
        
    }
}
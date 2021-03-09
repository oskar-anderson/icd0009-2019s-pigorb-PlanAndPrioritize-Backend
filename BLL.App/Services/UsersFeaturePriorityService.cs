using BLL.App.DTO;
using BLL.App.DTO.Mappers;
using Contracts.BLL.App.Services;
using Contracts.DAL.App;
using Contracts.DAL.App.Repositories;
using DAL.App.DTO;
using ee.itcollege.pigorb.bookswap.BLL.Base.Services;

namespace BLL.App.Services
{
    public class UsersFeaturePriorityService 
        : BaseEntityService<IUsersFeaturePriorityRepository, IAppUnitOfWork, UsersFeaturePriorityDalDto, UsersFeaturePriorityBllDto>, 
            IUsersFeaturePriorityService
    {
        private readonly BLLUsersFeaturePriorityMapper _mapper = new BLLUsersFeaturePriorityMapper();
        
        public UsersFeaturePriorityService(IAppUnitOfWork unitOfWork)
            : base(unitOfWork, new BLLUsersFeaturePriorityMapper(), unitOfWork.UsersFeaturePriorities)
        {
        }
        
    }
}
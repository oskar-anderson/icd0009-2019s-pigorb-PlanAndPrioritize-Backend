using System;
using System.Threading.Tasks;
using BLL.App.DTO;
using BLL.App.DTO.Mappers;
using Contracts.BLL.App.Services;
using Contracts.DAL.App;
using Contracts.DAL.App.Repositories;
using DAL.App.DTO;
using ee.itcollege.pigorb.bookswap.BLL.Base.Services;

namespace BLL.App.Services
{
    public class AppUserService : BaseEntityService<IAppUserRepository, IAppUnitOfWork, AppUserDalDto, AppUserBllDto>, IAppUserService
    {
        private readonly BLLAppUserMapper _mapper = new BLLAppUserMapper();
        
        public AppUserService(IAppUnitOfWork unitOfWork)
            : base(unitOfWork, new BLLAppUserMapper(), unitOfWork.AppUsers)
        {
        }
    }
}
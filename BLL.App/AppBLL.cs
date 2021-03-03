using Contracts.BLL.App;
using Contracts.DAL.App;
using ee.itcollege.pigorb.bookswap.BLL.Base;

namespace BLL.App
{
    public class AppBLL : BaseBLL<IAppUnitOfWork>, IAppBLL
    {
        public AppBLL(IAppUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        
        // public IAppUserService AppUsers => GetService<IAppUserService>(() => new AppUserService(UnitOfWork));
        // public IAuthorService Authors => GetService<IAuthorService>(() => new AuthorService(UnitOfWork));
    }
}

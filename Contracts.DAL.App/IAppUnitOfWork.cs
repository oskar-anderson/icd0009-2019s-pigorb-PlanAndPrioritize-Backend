using ee.itcollege.pigorb.bookswap.Contracts.DAL.Base;

namespace Contracts.DAL.App
{
    public interface IAppUnitOfWork : IBaseUnitOfWork, IBaseEntityTracker
    {
        //IAppUserRepository AppUsers { get; } // a factory is somewhere
        
        // IAuthorRepository Authors { get; }
        
    }
}

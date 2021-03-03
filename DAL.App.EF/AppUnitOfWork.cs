using System;
using Contracts.DAL.App;
using ee.itcollege.pigorb.bookswap.DAL.Base.EF;

namespace DAL.App.EF
{
    public class AppUnitOfWork : EFBaseUnitOfWork<Guid, AppDbContext>, IAppUnitOfWork
    {
        public AppUnitOfWork(AppDbContext uowDbContext) : base(uowDbContext)
        {
        }

        //public IAppUserRepository AppUsers => GetRepository<IAppUserRepository>(
        //    () => new AppUserRepository(UowDbContext));
        
        // public IAuthorRepository Authors => GetRepository<IAuthorRepository>(
        //    () => new AuthorRepository(UowDbContext));

    }
}
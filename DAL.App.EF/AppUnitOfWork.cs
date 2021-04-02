using System;
using Contracts.DAL.App;
using Contracts.DAL.App.Repositories;
using DAL.App.EF.Repositories;
using ee.itcollege.pigorb.bookswap.DAL.Base.EF;

namespace DAL.App.EF
{
    public class AppUnitOfWork : EFBaseUnitOfWork<Guid, AppDbContext>, IAppUnitOfWork
    {
        public AppUnitOfWork(AppDbContext uowDbContext) : base(uowDbContext)
        {
        }

        public IAppUserRepository AppUsers => GetRepository<IAppUserRepository>(
            () => new AppUserRepository(UowDbContext));
        public ICategoryRepository Categories => GetRepository<ICategoryRepository>(
            () => new CategoryRepository(UowDbContext));
        public ICommentRepository Comments => GetRepository<ICommentRepository>(
            () => new CommentRepository(UowDbContext));
        public IFeatureInVotingRepository FeatureInVotings => GetRepository<IFeatureInVotingRepository>(
            () => new FeatureInVotingRepository(UowDbContext));
        public IFeatureRepository Features => GetRepository<IFeatureRepository>(
            () => new FeatureRepository(UowDbContext));
        public IUserInVotingRepository UserInVotings => GetRepository<IUserInVotingRepository>(
            () => new UserInVotingRepository(UowDbContext));
        public IUsersFeaturePriorityRepository UsersFeaturePriorities => GetRepository<IUsersFeaturePriorityRepository>(
            () => new UsersFeaturePriorityRepository(UowDbContext));
        public IVotingRepository Votings => GetRepository<IVotingRepository>(
            () => new VotingRepository(UowDbContext));
    }
}
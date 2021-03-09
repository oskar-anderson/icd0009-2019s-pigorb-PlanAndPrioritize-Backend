using BLL.App.Services;
using Contracts.BLL.App;
using Contracts.BLL.App.Services;
using Contracts.DAL.App;
using ee.itcollege.pigorb.bookswap.BLL.Base;

namespace BLL.App
{
    public class AppBLL : BaseBLL<IAppUnitOfWork>, IAppBLL
    {
        public AppBLL(IAppUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        
        public IAppUserService AppUsers 
            => GetService<IAppUserService>(() => new AppUserService(UnitOfWork));
        public ICategoryService Categories 
            => GetService<ICategoryService>(() => new CategoryService(UnitOfWork));
        public ICommentService Comments 
            => GetService<ICommentService>(() => new CommentService(UnitOfWork));
        public IFeatureInVotingService FeatureInVotings 
            => GetService<IFeatureInVotingService>(() => new FeatureInVotingService(UnitOfWork));
        public IFeatureService Features 
            => GetService<IFeatureService>(() => new FeatureService(UnitOfWork));
        public IFeatureStatusService FeatureStatuses 
            => GetService<IFeatureStatusService>(() => new FeatureStatusService(UnitOfWork));
        public IPriorityStatusService PriorityStatuses 
            => GetService<IPriorityStatusService>(() => new PriorityStatusService(UnitOfWork));
        public IUserInVotingService UserInVotings 
            => GetService<IUserInVotingService>(() => new UserInVotingService(UnitOfWork));
        public IUsersFeaturePriorityService UsersFeaturePriorities 
            => GetService<IUsersFeaturePriorityService>(() => new UsersFeaturePriorityService(UnitOfWork));
        public IVotingService Votings 
            => GetService<IVotingService>(() => new VotingService(UnitOfWork));
        public IVotingStatusService VotingStatuses 
            => GetService<IVotingStatusService>(() => new VotingStatusService(UnitOfWork));
    }
}

using Contracts.BLL.App.Services;
using ee.itcollege.pigorb.bookswap.Contracts.BLL.Base;

namespace Contracts.BLL.App
{
    public interface IAppBLL : IBaseBLL
    {
        public IAppUserService AppUsers { get; }
        public ICategoryService Categories { get; }
        public ICommentService Comments { get; }
        public IFeatureInVotingService FeatureInVotings { get; }
        public IFeatureService Features { get; }
        public IFeatureStatusService FeatureStatuses { get; }
        public IPriorityStatusService PriorityStatuses { get; }
        public IUserInVotingService UserInVotings { get; }
        public IUsersFeaturePriorityService UsersFeaturePriorities { get; }
        public IVotingService Votings { get; }
        public IVotingStatusService VotingStatuses { get; }
    }
}

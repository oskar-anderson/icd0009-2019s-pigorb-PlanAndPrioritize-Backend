using Contracts.DAL.App.Repositories;
using ee.itcollege.pigorb.bookswap.Contracts.DAL.Base;

namespace Contracts.DAL.App
{
    public interface IAppUnitOfWork : IBaseUnitOfWork, IBaseEntityTracker
    {
        IAppUserRepository AppUsers { get; }
        ICategoryRepository Categories { get; }
        ICommentRepository Comments { get; }
        IFeatureInVotingRepository FeatureInVotings { get; }
        IFeatureRepository Features { get; }
        IUserInVotingRepository UserInVotings { get; }
        IUsersFeaturePriorityRepository UsersFeaturePriorities { get; }
        IVotingRepository Votings { get; }
    }
}

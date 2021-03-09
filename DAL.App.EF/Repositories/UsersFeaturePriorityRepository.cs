using Contracts.DAL.App.Repositories;
using DAL.App.DTO;
using DAL.App.DTO.Mappers;
using Domain;
using ee.itcollege.pigorb.bookswap.DAL.Base.EF.Repositories;

namespace DAL.App.EF.Repositories
{
    public class UsersFeaturePriorityRepository 
        : EFBaseRepository<AppDbContext, UsersFeaturePriority, UsersFeaturePriorityDalDto>, IUsersFeaturePriorityRepository
    {
        private readonly DALUsersFeaturePriorityMapper _mapper = new DALUsersFeaturePriorityMapper();
        
        public UsersFeaturePriorityRepository(AppDbContext dbContext) : base(dbContext, new DALUsersFeaturePriorityMapper())
        {
        }
        
      }
}

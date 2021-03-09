using Contracts.DAL.App.Repositories;
using DAL.App.DTO;
using DAL.App.DTO.Mappers;
using Domain;
using ee.itcollege.pigorb.bookswap.DAL.Base.EF.Repositories;

namespace DAL.App.EF.Repositories
{
    public class PriorityStatusRepository : EFBaseRepository<AppDbContext, PriorityStatus, PriorityStatusDalDto>, IPriorityStatusRepository
    {
        private readonly DALPriorityStatusMapper _mapper = new DALPriorityStatusMapper();
        
        public PriorityStatusRepository(AppDbContext dbContext) : base(dbContext, new DALPriorityStatusMapper())
        {
        }
        
      }
}

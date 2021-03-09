using Contracts.DAL.App.Repositories;
using DAL.App.DTO;
using DAL.App.DTO.Mappers;
using Domain;
using ee.itcollege.pigorb.bookswap.DAL.Base.EF.Repositories;

namespace DAL.App.EF.Repositories
{
    public class CategoryRepository : EFBaseRepository<AppDbContext, Category, CategoryDalDto>, ICategoryRepository
    {
        private readonly DALCategoryMapper _mapper = new DALCategoryMapper();
        
        public CategoryRepository(AppDbContext dbContext) : base(dbContext, new DALCategoryMapper())
        {
        }
        
      }
}

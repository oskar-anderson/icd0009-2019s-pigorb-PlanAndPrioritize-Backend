using BLL.App.DTO;
using BLL.App.DTO.Mappers;
using Contracts.BLL.App.Services;
using Contracts.DAL.App;
using Contracts.DAL.App.Repositories;
using DAL.App.DTO;
using ee.itcollege.pigorb.bookswap.BLL.Base.Services;

namespace BLL.App.Services
{
    public class CategoryService : BaseEntityService<ICategoryRepository, IAppUnitOfWork, CategoryDalDto, CategoryBllDto>, ICategoryService
    {
        private readonly BLLCategoryMapper _mapper = new BLLCategoryMapper();
        
        public CategoryService(IAppUnitOfWork unitOfWork)
            : base(unitOfWork, new BLLCategoryMapper(), unitOfWork.Categories)
        {
        }
        
    }
}
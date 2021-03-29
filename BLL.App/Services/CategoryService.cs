using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<IEnumerable<CategoryBllDto>> GetAll()
        {
            return (await ServiceRepository.GetAll()).Select(dalEntity => _mapper.Map(dalEntity));
        }

        public async Task<bool> Exists(Guid id)
        {
            return await ServiceRepository.Exists(id);
        }

        public async Task<CategoryBllDto> FirstOrDefault(Guid id)
        {
            return _mapper.Map(await ServiceRepository.FirstOrDefault(id));
        }

        public async Task Delete(Guid id)
        {
            await ServiceRepository.Delete(id);
        }

        public CategoryBllDto Edit(CategoryBllDto entity)
        {
            return _mapper.Map(ServiceRepository.Edit(_mapper.Map(entity)));
        }
    }
}
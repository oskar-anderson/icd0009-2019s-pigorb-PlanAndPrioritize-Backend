using BLL.App.DTO;
using DAL.App.DTO;

namespace API.DTO.v1.Mappers
{
    public class DTOCategoryMapper : DTOAppMapper<CategoryBllDto, CategoryApiDto>
    {
        public CategoryBllDto MapCategoryCreate(CategoryCreateApiDto apiDTO)
        {
            return Mapper.Map<CategoryCreateApiDto, CategoryBllDto>(apiDTO);
        }
        
        public CategoryBllDto MapCategoryEdit(CategoryEditApiDto apiDTO)
        {
            return Mapper.Map<CategoryEditApiDto, CategoryBllDto>(apiDTO);
        }
    }
}
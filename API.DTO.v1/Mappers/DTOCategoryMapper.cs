using BLL.App.DTO;

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
            return new CategoryBllDto
            {
                Id = apiDTO.Id,
                Title = apiDTO.Title,
                Description = apiDTO.Description
            };
        }
    }
}
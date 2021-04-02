using System.Linq;
using Domain;

namespace DAL.App.DTO.Mappers
{
    public class DALCategoryMapper : DALAppMapper<Category, CategoryDalDto>
    {
        public CategoryDalDto MapCategory(Category category)
        {
            return new CategoryDalDto
            {
                Id = category.Id,
                Title = category.Title,
                Description = category.Description,
                Features = category.Features?.Select(feature => new FeatureDalDto
                {
                    Id = category.Id,
                    Title = feature.Title,
                    Size = feature.Size,
                    PriorityValue = feature.PriorityValue,
                    Description = feature.Description,
                    StartTime = feature.StartTime,
                    EndTime = feature.EndTime,
                    Duration = feature.Duration,
                    CategoryId = feature.CategoryId,
                    FeatureStatus = feature.FeatureStatus,
                    AppUserId = feature.AppUserId,
                    TimeCreated = feature.TimeCreated,
                    LastEdited = feature.LastEdited,
                    ChangeLog = feature.ChangeLog
                }).ToList()
            };
        }
    }
}
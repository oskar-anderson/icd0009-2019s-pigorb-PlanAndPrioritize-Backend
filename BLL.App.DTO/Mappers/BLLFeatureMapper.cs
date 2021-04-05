using System.Linq;
using DAL.App.DTO;

namespace BLL.App.DTO.Mappers
{
    public class BLLFeatureMapper : BLLAppMapper<FeatureDalDto, FeatureBllDto>
    {
        public FeatureBllDto MapFeature(FeatureDalDto feature)
        {
            return new FeatureBllDto
            {
                Id = feature.Id,
                Title = feature.Title,
                Size = feature.Size,
                PriorityValue = feature.PriorityValue,
                Description = feature.Description,
                StartTime = feature.StartTime,
                EndTime = feature.EndTime,
                Duration = feature.Duration,
                CategoryId = feature.CategoryId,
                Category = new CategoryBllDto
                {
                    Id = feature.Category!.Id,
                    Title = feature.Category.Title,
                    Description = feature.Category.Description
                },
                FeatureStatus = feature.FeatureStatus,
                AppUserId = feature.AppUserId,
                AppUser = CreateAppUserBllDto(feature.AppUser),
                TimeCreated = feature.TimeCreated,
                CreatedById = feature.CreatedById,
                CreatedBy = CreateAppUserBllDto(feature.CreatedBy),
                LastEdited = feature.LastEdited,
                ChangeLog = feature.ChangeLog,
                Comments = feature.Comments?.Select(c => new CommentBllDto
                {
                    Id = c.Id,
                    Content = c.Content,
                    FeatureId = c.FeatureId,
                    AppUserId = c.AppUserId,
                    AppUser = new AppUserBllDto
                    {
                        Id = c.AppUser!.Id,
                        FirstName = c.AppUser.FirstName,
                        LastName = c.AppUser.LastName
                    }
                }).ToList(),
                FeatureInVotings = feature.FeatureInVotings?.Select(f => new FeatureInVotingBllDto
                {
                    Id = f.Id,
                    AverageSize = f.AverageSize,
                    AverageTimeCriticality = f.AverageTimeCriticality,
                    AverageBusinessValue = f.AverageBusinessValue,
                    AverageRiskOrOpportunity = f.AverageRiskOrOpportunity,
                    AveragePriorityValue = f.AveragePriorityValue,
                    VotingId = f.VotingId,
                    Voting = new VotingBllDto
                    {
                     Id = f.Voting!.Id,
                     Title = f.Voting.Title,
                     Description = f.Voting.Description,
                     StartTime = f.Voting.StartTime,
                     EndTime = f.Voting.EndTime
                    },
                    FeatureId = f.FeatureId
                }).ToList()
            };
        }
        
        private AppUserBllDto? CreateAppUserBllDto(AppUserDalDto? appUser)
        {
            if (appUser == null)
            {
                return null;
            }

            return new AppUserBllDto
            {
                Id = appUser.Id,
                FirstName = appUser.FirstName,
                LastName = appUser.LastName
            };
        }
    }
}
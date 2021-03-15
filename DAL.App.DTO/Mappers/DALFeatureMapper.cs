using System.Linq;
using Domain;

namespace DAL.App.DTO.Mappers
{
    public class DALFeatureMapper : DALAppMapper<Feature, FeatureDalDto>
    {
        public FeatureDalDto MapFeature(Feature feature)
        {
            return new FeatureDalDto
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
                Category = new CategoryDalDto
                {
                    Id = feature.Category!.Id,
                    Title = feature.Category.Title,
                    Description = feature.Category.Description
                },
                FeatureStatusId = feature.FeatureStatusId,
                FeatureStatus = new FeatureStatusDalDto
                {
                    Id = feature.FeatureStatus!.Id,
                    Name = feature.FeatureStatus.Name,
                    Description = feature.FeatureStatus.Description
                },
                AppUserId = feature.AppUserId,
                AppUser = CreateAppUserDalDto(feature.AppUser),
                TimeCreated = feature.TimeCreated,
                LastEdited = feature.LastEdited,
                ChangeLog = feature.ChangeLog,
                Comments = feature.Comments?.Select(c => new CommentDalDto
                {
                    Id = c.Id,
                    Content = c.Content,
                    FeatureId = c.FeatureId,
                    AppUserId = c.AppUserId,
                    AppUser = new AppUserDalDto
                    {
                        Id = c.AppUser!.Id,
                        FirstName = c.AppUser.FirstName,
                        LastName = c.AppUser.LastName
                    }
                }).ToList(),
                FeatureInVotings = feature.FeatureInVotings?.Select(f => new FeatureInVotingDalDto
                {
                    Id = f.Id,
                    AverageSize = f.AverageSize,
                    AverageTimeCriticality = f.AverageTimeCriticality,
                    AverageBusinessValue = f.AverageBusinessValue,
                    AverageRiskOrOpportunity = f.AverageRiskOrOpportunity,
                    AveragePriorityValue = f.AveragePriorityValue,
                    VotingId = f.VotingId,
                    Voting = new VotingDalDto
                    {
                     Id = f.Voting!.Id,
                     Title = f.Voting.Title,
                     Description = f.Voting.Description,
                     StartTime = f.Voting.StartTime,
                     EndTime = f.Voting.EndTime,
                     VotingStatusId = f.Voting.VotingStatusId,
                     // VotingStatus = new VotingStatusDalDto
                     // {
                     //     Id = f.Voting!.VotingStatus!.Id,
                     //     Name = f.Voting.VotingStatus.Name,
                     //     Description = f.Voting.VotingStatus.Description
                     // }
                    },
                    FeatureId = f.FeatureId
                }).ToList()
            };
        }
        
        private AppUserDalDto? CreateAppUserDalDto(AppUser? appUser)
        {
            if (appUser == null)
            {
                return null;
            }

            return new AppUserDalDto
            {
                Id = appUser.Id,
                FirstName = appUser.FirstName,
                LastName = appUser.LastName
            };
        }
    }
}
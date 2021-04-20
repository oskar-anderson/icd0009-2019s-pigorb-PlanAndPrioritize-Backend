using Domain;

namespace DAL.App.DTO.Mappers
{
    public class DALUsersFeaturePriorityMapper : DALAppMapper<UsersFeaturePriority, UsersFeaturePriorityDalDto>
    {
        public UsersFeaturePriorityDalDto MapUserPriorityWithSubData(UsersFeaturePriority domainEntity)
        {
            return new UsersFeaturePriorityDalDto
            {
                Id = domainEntity.Id,
                Size = domainEntity.Size,
                PriorityValue = domainEntity.PriorityValue,
                BusinessValue = domainEntity.BusinessValue,
                TimeCriticality = domainEntity.TimeCriticality,
                RiskOrOpportunity = domainEntity.RiskOrOpportunity,
                AppUserId = domainEntity.AppUserId,
                AppUser = CreateAppUserDalDto(domainEntity.AppUser),
                FeatureInVotingId = domainEntity.FeatureInVotingId,
                FeatureInVoting = CreateFeatureInVotingDalDto(domainEntity.FeatureInVoting)
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
        
        private FeatureInVotingDalDto? CreateFeatureInVotingDalDto(FeatureInVoting? featureInVoting)
        {
            if (featureInVoting == null)
            {
                return null;
            }

            return new FeatureInVotingDalDto
            {
                Id = featureInVoting.Id,
                FeatureId = featureInVoting.FeatureId,
                Feature = new FeatureDalDto
                {
                    Id = featureInVoting.Feature!.Id,
                    Title = featureInVoting.Feature!.Title,
                    Size = featureInVoting.Feature!.Size,
                    PriorityValue = featureInVoting.Feature!.PriorityValue,
                    Description = featureInVoting.Feature!.Description,
                    StartTime = featureInVoting.Feature!.StartTime,
                    EndTime = featureInVoting.Feature!.EndTime,
                    Duration = featureInVoting.Feature!.Duration,
                    CategoryId = featureInVoting.Feature!.CategoryId,
                    Category = new CategoryDalDto
                    {
                        Id = featureInVoting.Feature.Category!.Id, 
                        Title = featureInVoting.Feature.Category.Title,  
                        Description = featureInVoting.Feature.Category.Description
                    },
                    FeatureStatus = featureInVoting.Feature!.FeatureStatus,
                    AppUserId = featureInVoting.Feature!.AppUserId,
                    TimeCreated = featureInVoting.Feature!.TimeCreated,
                    CreatedById = featureInVoting.Feature!.CreatedById,
                    LastEdited = featureInVoting.Feature!.LastEdited,
                    ChangeLog = featureInVoting.Feature!.ChangeLog
                },
                VotingId = featureInVoting.VotingId
            };
        }

        public UsersFeaturePriorityDalDto MapUserPriority(UsersFeaturePriority domainEntity)
        {
            return new UsersFeaturePriorityDalDto
            {
                Id = domainEntity.Id,
                Size = domainEntity.Size,
                PriorityValue = domainEntity.PriorityValue,
                BusinessValue = domainEntity.BusinessValue,
                TimeCriticality = domainEntity.TimeCriticality,
                RiskOrOpportunity = domainEntity.RiskOrOpportunity,
                AppUserId = domainEntity.AppUserId,
                AppUser = CreateAppUserDalDto(domainEntity.AppUser),
                FeatureInVotingId = domainEntity.FeatureInVotingId
            };
        }
    }
}
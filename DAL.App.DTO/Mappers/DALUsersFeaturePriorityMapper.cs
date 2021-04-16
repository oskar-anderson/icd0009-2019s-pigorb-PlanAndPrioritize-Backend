using Domain;

namespace DAL.App.DTO.Mappers
{
    public class DALUsersFeaturePriorityMapper : DALAppMapper<UsersFeaturePriority, UsersFeaturePriorityDalDto>
    {
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
                PriorityStatus = domainEntity.PriorityStatus,
                AppUserId = domainEntity.AppUserId,
                AppUser = CreateAppUserDalDto(domainEntity.AppUser),
                FeatureInVotingId = domainEntity.FeatureInVotingId
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
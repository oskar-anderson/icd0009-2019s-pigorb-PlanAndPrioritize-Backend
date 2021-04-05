using System.Linq;
using Domain;

namespace DAL.App.DTO.Mappers
{
    public class DALVotingMapper : DALAppMapper<Voting, VotingDalDto>
    {
        public VotingDalDto MapVoting(Voting voting)
        {
            return new VotingDalDto
            {
                Id = voting.Id,
                Title = voting.Title,
                Description = voting.Description,
                StartTime = voting.StartTime,
                EndTime = voting.EndTime,
                UserInVotings = voting.UserInVotings?.Select(u => new UserInVotingDalDto
                {
                    Id = u.Id,
                    AppUserId = u.AppUserId,
                    AppUser = new AppUserDalDto
                    {
                        Id = u.AppUser!.Id,
                        FirstName = u.AppUser.FirstName,
                        LastName = u.AppUser.LastName
                    }
                }).ToList(),
                FeatureInVotings = voting.FeatureInVotings?.Select(u => new FeatureInVotingDalDto
                {
                    Id = u.Id,
                    FeatureId = u.FeatureId,
                    Feature = new FeatureDalDto
                    {
                        Id = u.Feature!.Id,
                        Title = u.Feature!.Title,
                        Size = u.Feature!.Size,
                        PriorityValue = u.Feature!.PriorityValue,
                        Description = u.Feature!.Description,
                        StartTime = u.Feature!.StartTime,
                        EndTime = u.Feature!.EndTime,
                        Duration = u.Feature!.Duration,
                        CategoryId = u.Feature!.CategoryId,
                        FeatureStatus = u.Feature!.FeatureStatus,
                        AppUserId = u.Feature!.AppUserId,
                        TimeCreated = u.Feature!.TimeCreated,
                        CreatedById = u.Feature!.CreatedById,
                        LastEdited = u.Feature!.LastEdited,
                        ChangeLog = u.Feature!.ChangeLog,
                    }
                }).ToList(),
            };
        }
    }
}
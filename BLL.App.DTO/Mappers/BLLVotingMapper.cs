using System.Linq;
using Classifiers;
using DAL.App.DTO;

namespace BLL.App.DTO.Mappers
{
    public class BLLVotingMapper : BLLAppMapper<VotingDalDto, VotingBllDto>
    {
        public VotingBllDto MapVoting(VotingDalDto dalDto)
        {
            return new VotingBllDto
            {
                Id = dalDto.Id,
                Title = dalDto.Title,
                Description = dalDto.Description,
                StartTime = dalDto.StartTime,
                EndTime = dalDto.EndTime,
                VotingStatus = VotingStatus.NotOpenYet,
                UserInVotings = dalDto.UserInVotings?.Select(u => new UserInVotingBllDto
                {
                    Id = u.Id,
                    AppUserId = u.AppUserId,
                    AppUser = new AppUserBllDto
                    {
                        Id = u.AppUser!.Id,
                        FirstName = u.AppUser.FirstName,
                        LastName = u.AppUser.LastName
                    }
                }).ToList(),
                FeatureInVotings = dalDto.FeatureInVotings?.Select(u => new FeatureInVotingBllDto
                {
                    Id = u.Id,
                    FeatureId = u.FeatureId,
                    Feature = new FeatureBllDto
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
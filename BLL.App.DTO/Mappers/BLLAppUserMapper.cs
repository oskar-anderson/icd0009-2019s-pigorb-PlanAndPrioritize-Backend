using System.Linq;
using DAL.App.DTO;

namespace BLL.App.DTO.Mappers
{
    public class BLLAppUserMapper : BLLAppMapper<AppUserDalDto, AppUserBllDto>
    {
        public AppUserBllDto MapUser(AppUserDalDto dalEntity)
        {
            return new AppUserBllDto
            {
                Id = dalEntity.Id,
                FirstName = dalEntity.FirstName,
                LastName = dalEntity.LastName,
                Email = dalEntity.Email,
                UserInVotings = dalEntity.UserInVotings?.Select(uv => new UserInVotingBllDto
                {
                    Id = uv.Id,
                    AppUserId = uv.AppUserId,
                    VotingId = uv.VotingId
                }).ToList(),
            };
        }
    }
}
using System.Linq;
using Domain;

namespace DAL.App.DTO.Mappers
{
    public class DALAppUserMapper : DALAppMapper<AppUser, AppUserDalDto>
    {
        public AppUserDalDto MapUser(AppUser entity)
        {
            return new AppUserDalDto
            {
                Id = entity.Id,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Email = entity.Email,
                UserInVotings = entity.UserInVotings?.Select(uv => new UserInVotingDalDto
                {
                    Id = uv.Id,
                    AppUserId = uv.AppUserId,
                    VotingId = uv.VotingId
                }).ToList(),
            };
        }
    }
}
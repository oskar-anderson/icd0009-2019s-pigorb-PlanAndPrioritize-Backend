using BLL.App.DTO;
using Classifiers;

namespace API.DTO.v1.Mappers
{
    public class DTOUsersFeaturePriorityMapper : DTOAppMapper<UsersFeaturePriorityBllDto, UsersFeaturePriorityApiDto>
    {
        public UsersFeaturePriorityApiDto MapUserPriority(UsersFeaturePriorityBllDto bllDto)
        {
            return new UsersFeaturePriorityApiDto
            {
                Id = bllDto.Id,
                Size = bllDto.Size,
                BusinessValue = bllDto.BusinessValue,
                TimeCriticality = bllDto.TimeCriticality,
                RiskOrOpportunity = bllDto.RiskOrOpportunity,
                PriorityValue = bllDto.PriorityValue,
                UserName = bllDto.AppUser?.FirstName + ' ' + bllDto.AppUser?.LastName
            };
        }
    }
}
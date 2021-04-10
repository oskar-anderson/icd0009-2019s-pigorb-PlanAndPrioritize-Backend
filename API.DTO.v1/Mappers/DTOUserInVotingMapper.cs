using BLL.App.DTO;

namespace API.DTO.v1.Mappers
{
    public class DTOUserInVotingMapper : DTOAppMapper<UserInVotingBllDto, UserInVotingApiDto>
    {
        public UserInVotingBllDto MapUserInVotingCreate(UserInVotingCreateApiDto apiDTO)
        {
            return Mapper.Map<UserInVotingCreateApiDto, UserInVotingBllDto>(apiDTO);
        }
    }
}
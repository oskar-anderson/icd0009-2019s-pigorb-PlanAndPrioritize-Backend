using BLL.App.DTO;

namespace API.DTO.v1.Mappers
{
    public class DTOFeatureInVotingMapper : DTOAppMapper<FeatureInVotingBllDto, FeatureInVotingApiDto>
    {
        public FeatureInVotingBllDto MapFeatureInVotingCreate(FeatureInVotingCreateApiDto apiDTO)
        {
            return Mapper.Map<FeatureInVotingCreateApiDto, FeatureInVotingBllDto>(apiDTO);
        }
    }
}
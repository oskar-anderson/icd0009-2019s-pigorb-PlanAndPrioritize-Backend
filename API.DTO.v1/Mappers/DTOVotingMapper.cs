using BLL.App.DTO;
using Classifiers;

namespace API.DTO.v1.Mappers
{
    public class DTOVotingMapper : DTOAppMapper<VotingBllDto, VotingApiDto>
    {
        public VotingBllDto MapVotingCreate(VotingCreateApiDto apiDTO)
        {
            return Mapper.Map<VotingCreateApiDto, VotingBllDto>(apiDTO);
        }
        
        public VotingApiDto MapVoting(VotingBllDto bllDTO)
        {
            var voting = Mapper.Map<VotingBllDto, VotingApiDto>(bllDTO);
            voting.VotingStatus = MapFromVotingStatus(bllDTO.VotingStatus);
            
            return voting;
        }
        
        private string MapFromVotingStatus(VotingStatus status)
        {
            return status switch
            {
                VotingStatus.NotOpenYet => "Not open yet",
                VotingStatus.Open => "open",
                VotingStatus.Closed => "Closed",
                _ => "Incorrect feature status value"
            };
        }
    }
}
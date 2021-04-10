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
        
        public VotingEditApiDto MapVotingEdit(VotingEditBllDto bllDTO)
        {
            var voting = Mapper.Map<VotingEditBllDto, VotingEditApiDto>(bllDTO);
            voting.VotingStatus = MapFromVotingStatus(bllDTO.VotingStatus);
            
            return voting;
        }
        
        public VotingBllDto MapFromVotingEdit(VotingEditApiDto apiDto)
        {
            return new VotingBllDto
            {
                Id = apiDto.Id,
                Title = apiDto.Title,
                Description = apiDto.Description,
                StartTime = apiDto.StartTime,
                EndTime = apiDto.EndTime,
                VotingStatus = MapToVotingStatus(apiDto.VotingStatus)
            };
        }

        private VotingStatus MapToVotingStatus(string status)
        {
            return status switch
            {
                "Not open yet" => VotingStatus.NotOpenYet,
                "Open" => VotingStatus.Open,
                "Closed" => VotingStatus.Closed,
                _ => VotingStatus.NotOpenYet
            };
        }

        private string MapFromVotingStatus(VotingStatus status)
        {
            return status switch
            {
                VotingStatus.NotOpenYet => "Not open yet",
                VotingStatus.Open => "Open",
                VotingStatus.Closed => "Closed",
                _ => "Incorrect feature status value"
            };
        }
    }
}
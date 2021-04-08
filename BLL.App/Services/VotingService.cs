using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.App.DTO;
using BLL.App.DTO.Mappers;
using Classifiers;
using Contracts.BLL.App.Services;
using Contracts.DAL.App;
using Contracts.DAL.App.Repositories;
using DAL.App.DTO;
using ee.itcollege.pigorb.bookswap.BLL.Base.Services;

namespace BLL.App.Services
{
    public class VotingService : BaseEntityService<IVotingRepository, IAppUnitOfWork, VotingDalDto, VotingBllDto>, IVotingService
    {
        private readonly BLLVotingMapper _mapper = new BLLVotingMapper();
        
        public VotingService(IAppUnitOfWork unitOfWork)
            : base(unitOfWork, new BLLVotingMapper(), unitOfWork.Votings)
        {
        }

        public async Task<IEnumerable<VotingBllDto>> GetAll()
        {
            return (await ServiceRepository.GetAll()).Select(dalEntity => WithVotingStatus(_mapper.MapVoting(dalEntity)));
        }

        public async Task<IEnumerable<VotingBllDto>> GetActiveVotings()
        {
            return (await ServiceRepository.GetActiveVotings()).Select(dalEntity => _mapper.Map(dalEntity));
        }

        public async Task<IEnumerable<VotingBllDto>> GetAllPlain()
        {
            return (await ServiceRepository.GetAllPlain()).Select(dalEntity => WithVotingStatus(_mapper.Map(dalEntity)));
        }

        public async Task<bool> Exists(Guid id)
        {
            return await ServiceRepository.Exists(id);
        }

        public async Task<VotingBllDto> FirstOrDefault(Guid id)
        {
            return WithVotingStatus(_mapper.MapVoting(await ServiceRepository.FirstOrDefault(id)));
        }

        public async Task Delete(Guid id)
        {
            await ServiceRepository.Delete(id);
        }

        public VotingBllDto Edit(VotingBllDto entity)
        {
            return _mapper.Map(ServiceRepository.Edit(_mapper.Map(entity)));
        }

        public async Task<IEnumerable<VotingBllDto>> GetVotingsForFeature(Guid featureId)
        {
            var votings = (await ServiceRepository.GetAll()).Select(dalEntity => _mapper.MapVoting(dalEntity));
            return votings.ToList().FindAll(v => HasFeature(v, featureId));
        }
        
        private bool HasFeature(VotingBllDto feature, Guid featureId)
        {
            return feature.FeatureInVotings != null && feature.FeatureInVotings.ToList().Any(f => f.FeatureId == featureId);
        }

        private VotingBllDto WithVotingStatus(VotingBllDto voting)
        {
            if (voting.StartTime < DateTime.Now && voting.EndTime > DateTime.Now)
            {
                voting.VotingStatus = VotingStatus.Open;
            }
            else if (voting.StartTime < DateTime.Now && voting.EndTime < DateTime.Now)
            {
                voting.VotingStatus = VotingStatus.Closed;
            }
            else
            {
                voting.VotingStatus = VotingStatus.NotOpenYet;
            }
            return voting;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.App.DTO;
using BLL.App.DTO.Mappers;
using Contracts.BLL.App.Services;
using Contracts.DAL.App;
using Contracts.DAL.App.Repositories;
using DAL.App.DTO;
using ee.itcollege.pigorb.bookswap.BLL.Base.Services;

namespace BLL.App.Services
{
    public class FeatureService : BaseEntityService<IFeatureRepository, IAppUnitOfWork, FeatureDalDto, FeatureBllDto>, IFeatureService
    {
        private readonly BLLFeatureMapper _mapper = new ();
        
        public FeatureService(IAppUnitOfWork unitOfWork)
            : base(unitOfWork, new BLLFeatureMapper(), unitOfWork.Features)
        {
        }

        public async Task<IEnumerable<FeatureBllDto>> GetAll()
        {
            return (await ServiceRepository.GetAll()).Select(dalEntity => _mapper.Map(dalEntity));
        }

        public async Task<IEnumerable<FeatureBllDto>> GetAllWithoutCollections()
        {
            return (await ServiceRepository.GetAll()).Select(dalEntity => _mapper.Map(dalEntity));
        }

        public async Task<bool> Exists(Guid id)
        {
            return await ServiceRepository.Exists(id);
        }

        public async Task<FeatureBllDto> FirstOrDefault(Guid id)
        {
            return _mapper.Map(await ServiceRepository.FirstOrDefault(id));
        }

        public async Task Delete(Guid id)
        {
            await ServiceRepository.Delete(id);
        }

        public FeatureBllDto Edit(FeatureBllDto entity)
        {
            return _mapper.Map(ServiceRepository.Edit(_mapper.Map(entity)));
        }
    }
}
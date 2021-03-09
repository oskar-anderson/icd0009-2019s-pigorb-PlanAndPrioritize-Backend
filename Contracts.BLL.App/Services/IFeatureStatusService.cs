using BLL.App.DTO;
using Contracts.DAL.App.Repositories;

namespace Contracts.BLL.App.Services
{
    public interface IFeatureStatusService : IFeatureStatusRepository<FeatureStatusBllDto>
    {
        
    }
}
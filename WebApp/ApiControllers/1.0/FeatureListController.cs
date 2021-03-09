using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTO.v1;
using API.DTO.v1.Mappers;
using Contracts.BLL.App;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.ApiControllers._1._0
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class FeatureListController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly DTOFeatureMapper _mapper;

        public FeatureListController(IAppBLL bll)
        {
            _bll = bll;
            _mapper = new DTOFeatureMapper();
        }
        
        public async Task<ActionResult<IEnumerable<FeatureApiDto>>> GetFeatures()
        {
            var features = (await _bll.Features.AllAsync())
                .Select(bllEntity => _mapper.Map(bllEntity));
            
            return Ok(features);
        }
    }
}
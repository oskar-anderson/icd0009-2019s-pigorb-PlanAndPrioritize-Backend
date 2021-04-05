using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTO.v1;
using API.DTO.v1.Mappers;
using Contracts.BLL.App;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.ApiControllers._1._0
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AppUsersController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly DTOAppUserMapper _mapper;

        public AppUsersController(IAppBLL bll)
        {
            _bll = bll;
            _mapper = new DTOAppUserMapper();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<AppUserApiDto>>> GetUsersForVoting(Guid id)
        {
            var users = (await _bll.AppUsers.GetUsersForVoting(id))
                .Select(bllEntity => _mapper.Map(bllEntity));
            
            return Ok(users);
        }
    }
}

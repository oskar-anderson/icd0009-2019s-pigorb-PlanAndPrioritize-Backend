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

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="bll">Application Business Logic Layer Interface</param>
        public AppUsersController(IAppBLL bll)
        {
            _bll = bll;
            _mapper = new DTOAppUserMapper();
        }

        /// <summary>
        /// Get users that have been assigned to priority voting ordered by users lastname
        /// </summary>
        /// <param name="id">Voting id - Guid</param>
        /// <returns>Collection of users</returns>
        /// <response code="200">Collection of users was successfully retrieved.</response>
        /// <response code="401">Not authorized to see the data.</response>
        [ProducesResponseType(typeof(IEnumerable<AppUserApiDto>), 200)]
        [ProducesResponseType(401)]
        [Produces("application/json")]
        [Consumes("application/json")]
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<AppUserApiDto>>> GetUsersForVoting(Guid id)
        {
            var users = (await _bll.AppUsers.GetUsersForVoting(id))
                .OrderBy(u => u.LastName)
                .Select(bllEntity => _mapper.Map(bllEntity));

            return Ok(users);
        }

        /// <summary>
        /// Get users that have not been assigned to priority voting ordered by users lastname
        /// </summary>
        /// <param name="id">Voting id - Guid</param>
        /// <returns>Collection of users</returns>
        /// <response code="200">Collection of users was successfully retrieved.</response>
        /// <response code="401">Not authorized to see the data.</response>
        [ProducesResponseType(typeof(IEnumerable<AppUserApiDto>), 200)]
        [ProducesResponseType(401)]
        [Produces("application/json")]
        [Consumes("application/json")]
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<AppUserApiDto>>> GetUsersNotInVoting(Guid id)
        {
            var users = (await _bll.AppUsers.GetUsersNotInVoting(id))
                .OrderBy(u => u.LastName)
                .Select(bllEntity => _mapper.Map(bllEntity));

            return Ok(users);
        }
    }
}
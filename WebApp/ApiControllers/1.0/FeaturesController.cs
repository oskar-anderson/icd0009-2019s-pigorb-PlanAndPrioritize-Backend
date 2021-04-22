using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTO.v1;
using API.DTO.v1.Mappers;
using BLL.App.DTO;
using Contracts.BLL.App;
using Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.ApiControllers._1._0
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class FeaturesController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly DTOFeatureMapper _mapper;
        private readonly DTOUsersFeaturePriorityMapper _pMapper;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="bll">Application Business Logic Layer Interface</param>
        public FeaturesController(IAppBLL bll)
        {
            _bll = bll;
            _mapper = new DTOFeatureMapper();
            _pMapper = new DTOUsersFeaturePriorityMapper();
        }

        /// <summary>
        /// Get features with updated priority values ordered descending based on time created
        /// </summary>
        /// <param name="limit">Number of records returned</param>
        /// <param name="search">
        /// Phrase which feature title or description must contain.
        /// If left blank, all features will be returned.
        /// </param>
        /// <returns>Collection of features</returns>
        /// <response code="200">Collection of features was successfully retrieved.</response>
        /// <response code="401">Not authorized to see the data.</response>
        [ProducesResponseType( typeof( IEnumerable<FeatureApiDto> ), 200 )]
        [ProducesResponseType( 401 )]
        [Produces( "application/json" )]
        [Consumes("application/json")]
        [HttpGet("{limit}/{search?}")]
        public async Task<ActionResult<IEnumerable<FeatureApiDto>>> GetFeaturesForList(int limit, string? search)
        {
            await _bll.Features.UpdatePriorityForAllFeatures();
            await _bll.SaveChangesAsync();
            
            var features = _bll.Features.GetAllWithVotings(search, limit)
                .Select(bllEntity => _mapper.MapFeature(bllEntity));
            
            return Ok(features);
        }
        
        /// <summary>
        /// Get features in suitable format for Gantt graph ordered descending based on time created
        /// </summary>
        /// <param name="limit">Number of records returned</param>
        /// <returns>Collection of features</returns>
        /// <response code="200">Collection of features was successfully retrieved.</response>
        /// <response code="401">Not authorized to request data.</response>
        [ProducesResponseType( typeof( IEnumerable<FeatureForGraphApiDto> ), 200 )]
        [ProducesResponseType( 401 )]
        [Produces( "application/json" )]
        [Consumes("application/json")]
        [HttpGet("{limit}")]
        public async Task<ActionResult<IEnumerable<FeatureForGraphApiDto>>> GetFeaturesForGraph(int limit)
        {
            var features = (await _bll.Features.GetFeaturesForGraph(limit))
                .Select(bllEntity => _mapper.MapFeatureForGraph(bllEntity));
            
            return Ok(features);
        }
        
        /// <summary>
        /// Get features with status not started
        /// </summary>
        /// <returns>Collection of features</returns>
        /// <response code="200">Collection of features was successfully retrieved.</response>
        /// <response code="401">Not authorized to request data.</response>
        [ProducesResponseType( typeof( IEnumerable<FeatureApiDto> ), 200 )]
        [ProducesResponseType( 401 )]
        [Produces( "application/json" )]
        [Consumes("application/json")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FeatureApiDto>>> GetToDoFeatures()
        {
            var features = (await _bll.Features.GetToDoFeatures())
                .Select(bllEntity => _mapper.Map(bllEntity));
            
            return Ok(features);
        }
        
        /// <summary>
        /// Get features that are not added to voting with specified id
        /// </summary>
        /// <param name="id">Voting id - Guid</param>
        /// <returns>Collection of features</returns>
        /// <response code="200">Features were successfully retrieved.</response>
        /// <response code="401">Not authorized to request data.</response>
        [ProducesResponseType( typeof( IEnumerable<FeatureApiDto> ), 200)]
        [ProducesResponseType( 401 )]
        [Produces( "application/json" )]
        [Consumes("application/json")]
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<FeatureApiDto>>> GetFeaturesNotInVoting(Guid id)
        {
            var features = (await _bll.Features.GetToDoFeaturesNotInVoting(id))
                .Select(bllEntity => _mapper.MapFeature(bllEntity));
            
            return Ok(features);
        }
        
        /// <summary>
        /// Get features that are added to voting with specified id
        /// </summary>
        /// <param name="id">Voting id - Guid</param>
        /// <returns>Collection of features</returns>
        /// <response code="200">Features were successfully retrieved.</response>
        /// <response code="401">Not authorized to request data.</response>
        [ProducesResponseType( typeof( IEnumerable<FeatureApiDto> ), 200)]
        [ProducesResponseType( 401 )]
        [Produces( "application/json" )]
        [Consumes("application/json")]
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<FeatureApiDto>>> GetFeaturesForVoting(Guid id)
        {
            var features = (await _bll.Features.GetFeaturesForVoting(id))
                .Select(bllEntity => _mapper.MapFeature(bllEntity));
            
            return Ok(features);
        }
        
        /// <summary>
        /// Get features that are added to voting with specified id
        /// If features already have priority values they will be included
        /// </summary>
        /// <param name="id">Voting id - Guid</param>
        /// <returns>Collection of features</returns>
        /// <response code="200">Features were successfully retrieved.</response>
        /// <response code="401">Not authorized to request data.</response>
        [ProducesResponseType( typeof( IEnumerable<FeatureApiDto> ), 200)]
        [ProducesResponseType( 401 )]
        [Produces( "application/json" )]
        [Consumes("application/json")]
        [HttpGet("{id}")]
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<FeatureApiDto>>> GetFeaturesWithPriority(Guid id)
        {
            var features = (await _bll.Features.GetFeaturesForVoting(id))
                .Select(bllEntity => _mapper.MapFeature(bllEntity));
            
            return Ok(features);
        }
        
        /// <summary>
        /// Get features with priority values added by logged in user for specific voting
        /// </summary>
        /// <param name="votingId">Voting id - Guid</param>
        /// <returns>Collection of features with user priority values</returns>
        /// <response code="200">Features were successfully retrieved.</response>
        /// <response code="401">Not authorized to request data.</response>
        [ProducesResponseType( typeof( IEnumerable<FeatureWithUsersPriorityApiDto> ), 200)]
        [ProducesResponseType( 401 )]
        [Produces( "application/json" )]
        [Consumes("application/json")]
        [HttpGet("{votingId}")]
        public async Task<ActionResult<IEnumerable<FeatureWithUsersPriorityApiDto>>> GetFeaturesWithUsersPriorities(Guid votingId)
        {
            var userPriorities = 
                await _bll.UsersFeaturePriorities.GetPrioritiesForVotingAndUser(votingId, User.UserId());
            var features = (await _bll.Features.GetFeaturesWithUsersPriorities(userPriorities, votingId))
                .Select(bllEntity => _mapper.MapFeatureWithPriority(bllEntity));

            return Ok(features);
        }
        
        /// <summary>
        /// Get user priorities for specified feature and voting
        /// </summary>
        /// <param name="featureId">Feature id - Guid</param>
        /// <param name="votingId">Voting id - Guid</param>
        /// <returns>Collection of users priorities</returns>
        /// <response code="200">Users priorities were successfully retrieved.</response>
        /// <response code="401">Not authorized to request data.</response>
        [ProducesResponseType( typeof( IEnumerable<UsersFeaturePriorityApiDto> ), 200)]
        [ProducesResponseType( 401 )]
        [Produces( "application/json" )]
        [Consumes("application/json")]
        [HttpGet("{featureId}/{votingId}")]
        public async Task<ActionResult<IEnumerable<UsersFeaturePriorityApiDto>>> GetUserPriorities(Guid featureId, Guid votingId)
        {
            var userPriorities = (await _bll.UsersFeaturePriorities.GetAllForFeatureAndVoting(featureId, votingId))
                .Select(bllEntity => _pMapper.MapUserPriority(bllEntity));
            
            return Ok(userPriorities);
        }
        
        /// <summary>
        /// Get feature with updated priority for specified id
        /// </summary>
        /// <param name="id">Feature id - Guid</param>
        /// <returns>Feature object</returns>
        /// <response code="200">Feature was successfully retrieved.</response>
        /// <response code="401">Not authorized to request data.</response>
        /// <response code="404">Feature with specified id was not found.</response>
        [ProducesResponseType( typeof( FeatureApiDto ), 200)]
        [ProducesResponseType( 401 )]
        [ProducesResponseType( 404 )]
        [Produces( "application/json" )]
        [Consumes("application/json")]
        [HttpGet("{id}")]
        public async Task<ActionResult<FeatureApiDto>> GetFeature(Guid id)
        {
            await _bll.Features.UpdatePriorityForFeature(id);
            await _bll.SaveChangesAsync();
            
            var featureDto = _mapper.MapFeature(await _bll.Features.FirstOrDefault(id));

            if (featureDto == null)
            {
                return NotFound();
            }

            return Ok(featureDto);
        }
        
        /// <summary>
        /// Get feature for specified id with basic fields filled
        /// </summary>
        /// <param name="id">Feature id - Guid</param>
        /// <returns>Feature object</returns>
        /// <response code="200">Feature was successfully retrieved.</response>
        /// <response code="401">Not authorized to request data.</response>
        /// <response code="404">Feature with specified id was not found.</response>
        [ProducesResponseType( typeof( FeatureEditApiDto ), 200)]
        [ProducesResponseType( 401 )]
        [ProducesResponseType( 404 )]
        [Produces( "application/json" )]
        [Consumes("application/json")]
        [HttpGet("{id}")]
        public async Task<ActionResult<FeatureEditApiDto>> GetFeaturePlain(Guid id)
        {
            var featureDto = _mapper.MapFeatureEdit(await _bll.Features.GetFeaturePlain(id));

            if (featureDto == null)
            {
                return NotFound();
            }

            return Ok(featureDto);
        }
        
        /// <summary>
        /// Update feature with specified id
        /// </summary>
        /// <param name="id">Feature id - Guid</param>
        /// <param name="featureDto">Feature object with updated data</param>
        /// <returns>Action result</returns>
        /// <response code="204">Feature was successfully edited.</response>
        /// <response code="401">Not authorized to perform action.</response>
        /// <response code="400">Bad request.</response>
        /// <response code="404">Feature for specified id was not found.</response>
        [ProducesResponseType( 204)]
        [ProducesResponseType( 401 )]
        [ProducesResponseType( 403 )]
        [ProducesResponseType( 404 )]
        [Produces( "application/json" )]
        [Consumes("application/json")]
        [HttpPut("{id}")]
        public async Task<IActionResult> EditFeature(Guid id, FeatureEditApiDto featureDto)
        {
            if (id != featureDto.Id)
            {
                return BadRequest();
            }
            
            var feature = await _bll.Features.FirstOrDefault(id);
            if (feature == null)
            {
                return BadRequest();
            }

            var user = await _bll.AppUsers.FindAsync(User.UserId());
            var userName = user.FirstName + " " + user.LastName;
            var category = await _bll.Categories.FirstOrDefault(featureDto.CategoryId);
            AppUserBllDto? assignee = null;
            if (featureDto.AppUserId != null)
            {
                assignee = await _bll.AppUsers.FindAsync(featureDto.AppUserId);
            }
            
            feature = _bll.Features.ConstructEditedFeatureWithChangeLog(feature, featureDto, userName, category, assignee);
            _bll.Features.Edit(feature);
            
            try
            {
                await _bll.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _bll.Features.Exists(featureDto.Id))
                {
                    return NotFound();
                }
                throw;
            }
            return NoContent();
        }
        
        /// <summary>
        /// Create new feature and add metadata
        /// </summary>
        /// <param name="featureDto">New feature object</param>
        /// <returns>Created feature object</returns>
        /// <response code="201">Feature was successfully created.</response>
        /// <response code="401">Not authorized to perform action.</response>
        [ProducesResponseType( typeof( FeatureApiDto ), 201 )]
        [ProducesResponseType( 401 )]
        [Produces( "application/json" )]
        [Consumes("application/json")]
        [HttpPost]
        public async Task<ActionResult<FeatureApiDto>> CreateFeature(FeatureCreateApiDto featureDto)
        {
            var feature = _mapper.MapFeatureCreate(featureDto);
            _bll.Features.AddWithMetaData(feature, User.UserId());
            await _bll.SaveChangesAsync();
            var createdFeature = await _bll.Features.GetLatestFeature();

            return CreatedAtAction("GetFeature", new { id = createdFeature.Id, 
                version = HttpContext.GetRequestedApiVersion()?.ToString() ?? "0"}, createdFeature);
        }

        /// <summary>
        /// Delete feature with specified id
        /// </summary>
        /// <param name="id">Feature id - Guid</param>
        /// <returns>Action result</returns>
        /// <response code="200">Feature was successfully deleted.</response>
        /// <response code="401">Not authorized to perform action.</response>
        /// <response code="404">Feature with specified id was not found.</response>
        [ProducesResponseType( 200)]
        [ProducesResponseType( 401 )]
        [ProducesResponseType( 404 )]
        [Produces( "application/json" )]
        [Consumes("application/json")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFeature(Guid id)
        {
            var feature = await _bll.Features.FirstOrDefault(id);
            
            if (feature == null)
            {
                return NotFound();
            }

            _bll.Features.Remove(feature);
            await _bll.SaveChangesAsync();

            return Ok(feature);
        }

        /// <summary>
        /// Get list of possible feature state values
        /// </summary>
        /// <returns>Collection of feature states</returns>
        /// <response code="200">Collection of feature states was successfully retrieved.</response>
        /// <response code="401">Not authorized to see the data.</response>
        [ProducesResponseType( typeof( IEnumerable<FeatureStatusApiDto> ), 200 )]
        [ProducesResponseType( 401 )]
        [Produces( "application/json" )]
        [Consumes("application/json")]
        public ActionResult<IEnumerable<FeatureStatusApiDto>> GetFeatureStates()
        {
            ICollection<FeatureStatusApiDto> states = _bll.Features.GetFeatureStatuses();
            return Ok(states);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTO.v1;
using API.DTO.v1.Mappers;
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
    public class VotingsController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly DTOVotingMapper _mapper;
        private readonly DTOFeatureInVotingMapper _fMapper;
        private readonly DTOUserInVotingMapper _uMapper;
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="bll">Application Business Logic Layer Interface</param>
        public VotingsController(IAppBLL bll)
        {
            _bll = bll;
            _fMapper = new DTOFeatureInVotingMapper();
            _uMapper = new DTOUserInVotingMapper();
            _mapper = new DTOVotingMapper();
        }

        /// <summary>
        /// Get all priority votings ordered descending by start time
        /// </summary>
        /// <returns>Collection of votings</returns>
        /// <response code="200">Collection of votings was successfully retrieved.</response>
        /// <response code="401">Not authorized to see the data.</response>
        [ProducesResponseType( typeof( IEnumerable<VotingApiDto> ), 200 )]
        [ProducesResponseType( 401 )]
        [Produces( "application/json" )]
        [Consumes("application/json")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VotingApiDto>>> GetVotings()
        {
            var votings = (await _bll.Votings.GetAll())
                .Select(bllEntity => _mapper.MapVoting(bllEntity));
            
            return Ok(votings);
        }
        
        /// <summary>
        /// Get priority votings that have been assigned to logged in user
        /// </summary>
        /// <returns>Collection of votings</returns>
        /// <response code="200">Collection of votings was successfully retrieved.</response>
        /// <response code="401">Not authorized to see the data.</response>
        [ProducesResponseType( typeof( IEnumerable<VotingApiDto> ), 200 )]
        [ProducesResponseType( 401 )]
        [Produces( "application/json" )]
        [Consumes("application/json")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VotingApiDto>>> GetAssignedVotings()
        {
            var votings = (await _bll.Votings.GetAssignedVotings(User.UserId()))
                .Select(bllEntity => _mapper.MapVoting(bllEntity));
            
            return Ok(votings);
        }
        
        /// <summary>
        /// Get priority votings that have not finished (includes open and not yet open votings)
        /// </summary>
        /// <returns>Collection of votings</returns>
        /// <response code="200">Collection of votings was successfully retrieved.</response>
        /// <response code="401">Not authorized to see the data.</response>
        [ProducesResponseType( typeof( IEnumerable<VotingApiDto> ), 200 )]
        [ProducesResponseType( 401 )]
        [Produces( "application/json" )]
        [Consumes("application/json")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VotingApiDto>>> GetActiveVotings()
        {
            var votings = (await _bll.Votings.GetActiveVotings())
                .Select(bllEntity => _mapper.Map(bllEntity));
            
            return Ok(votings);
        }
        
        /// <summary>
        /// Get priority votings that have not finished (includes open and not yet open votings)
        /// and where feature with specified id has not been added
        /// </summary>
        /// <param name="id">Feature id - Guid</param>
        /// <returns>Collection of votings</returns>
        /// <response code="200">Collection of votings was successfully retrieved.</response>
        /// <response code="401">Not authorized to see the data.</response>
        [ProducesResponseType( typeof( IEnumerable<VotingApiDto> ), 200 )]
        [ProducesResponseType( 401 )]
        [Produces( "application/json" )]
        [Consumes("application/json")]
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<VotingApiDto>>> GetActiveVotingsNotInFeature(Guid id)
        {
            var votings = (await _bll.Votings.GetActiveVotingsNotInFeature(id))
                .Select(bllEntity => _mapper.MapVoting(bllEntity));
            
            return Ok(votings);
        }
        
        /// <summary>
        /// Get priority votings where feature with specified id has been added
        /// </summary>
        /// <param name="id">Feature id - Guid</param>
        /// <returns>Collection of votings</returns>
        /// <response code="200">Collection of votings was successfully retrieved.</response>
        /// <response code="401">Not authorized to see the data.</response>
        [ProducesResponseType( typeof( IEnumerable<VotingApiDto> ), 200 )]
        [ProducesResponseType( 401 )]
        [Produces( "application/json" )]
        [Consumes("application/json")]
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<VotingApiDto>>> GetVotingsForFeature(Guid id)
        {
            var votings = (await _bll.Votings.GetVotingsForFeature(id))
                .Select(bllEntity => _mapper.MapVoting(bllEntity));
            
            return Ok(votings);
        }

        /// <summary>
        /// Get priority voting for specified id
        /// </summary>
        /// <param name="id">Voting id - Guid</param>
        /// <returns>Voting object</returns>
        /// <response code="200">Voting was successfully retrieved.</response>
        /// <response code="401">Not authorized to see the data.</response>
        /// <response code="404">Voting with specified id was not found.</response>
        [ProducesResponseType( typeof( VotingApiDto ), 200 )]
        [ProducesResponseType( 401 )]
        [ProducesResponseType( 401 )]
        [Produces( "application/json" )]
        [Consumes("application/json")]
        [HttpGet("{id}")]
        public async Task<ActionResult<VotingApiDto>> GetVoting(Guid id)
        {
            var votingDto = _mapper.MapVoting(await _bll.Votings.FirstOrDefault(id));

            if (votingDto == null)
            {
                return NotFound();
            }

            return Ok(votingDto);
        }
        
        /// <summary>
        /// Get priority voting with lists of user and feature id-s
        /// </summary>
        /// <param name="id">Voting id</param>
        /// <returns>Voting object</returns>
        /// <response code="200">Voting was successfully retrieved.</response>
        /// <response code="401">Not authorized to see the data.</response>
        /// <response code="404">Voting with specified id was not found.</response>
        [ProducesResponseType( typeof( VotingEditApiDto ), 200 )]
        [ProducesResponseType( 401 )]
        [ProducesResponseType( 404 )]
        [Produces( "application/json" )]
        [Consumes("application/json")]
        [HttpGet("{id}")]
        public async Task<ActionResult<VotingEditApiDto>> GetVotingEdit(Guid id)
        {
            var votingDto = _mapper.MapVotingEdit(await _bll.Votings.GetVotingWithIdCollections(id));

            if (votingDto == null)
            {
                return NotFound();
            }

            return Ok(votingDto);
        }
        
        /// <summary>
        /// Get boolean value if signed in user has already assigned priorities for features
        /// during priority voting with specified id
        /// </summary>
        /// <param name="votingId">Voting id - Guid</param>
        /// <returns>boolean</returns>
        /// <response code="200">Boolean value was successfully retrieved.</response>
        /// <response code="401">Not authorized to see the data.</response>
        [ProducesResponseType( typeof( bool ), 200 )]
        [ProducesResponseType( 401 )]
        [Produces( "application/json" )]
        [Consumes("application/json")]
        [HttpGet("{votingId}")]
        public async Task<ActionResult<bool>> HasVoted(Guid votingId)
        {
            var hasVoted = await _bll.UsersFeaturePriorities.ExistsPrioritiesForVotingAndUser(votingId, User.UserId());
            return Ok(hasVoted);
        }
        
        /// <summary>
        /// Get boolean value if signed in user has priority votings assigned to him/her that are currently open for voting
        /// during priority voting with specified id
        /// </summary>
        /// <returns>boolean</returns>
        /// <response code="200">Boolean value was successfully retrieved.</response>
        /// <response code="401">Not authorized to see the data.</response>
        [ProducesResponseType( typeof( bool ), 200 )]
        [ProducesResponseType( 401 )]
        [Produces( "application/json" )]
        [Consumes("application/json")]
        [HttpGet]
        public async Task<ActionResult<bool>> HasAssignedOpenVotings()
        {
            var hasAssignedOpenVotings = await _bll.UserInVotings.HasAssignedOpenVotings(User.UserId());
            return Ok(hasAssignedOpenVotings);
        }
        
        /// <summary>
        /// Update voting with specified id
        /// </summary>
        /// <param name="id">Voting id - Guid</param>
        /// <param name="votingDto">Voting object with updated data</param>
        /// <returns>Action result</returns>
        /// <response code="204">Voting was successfully updated.</response>
        /// <response code="401">Not authorized to perform action.</response>
        /// <response code="400">Bad request.</response>
        /// <response code="404">Voting for specified id was not found.</response>
        [ProducesResponseType( 204)]
        [ProducesResponseType( 401 )]
        [ProducesResponseType( 403 )]
        [ProducesResponseType( 404 )]
        [Produces( "application/json" )]
        [Consumes("application/json")]
        [HttpPut("{id}")]
        public async Task<IActionResult> EditVoting(Guid id, VotingEditApiDto votingDto)
        {
            if (id != votingDto.Id)
            {
                return BadRequest();
            }
            
            var voting = await _bll.Votings.FirstOrDefault(id);
            if (voting == null)
            {
                return BadRequest();
            }

            voting = _mapper.MapFromVotingEdit(votingDto);
            _bll.Votings.Edit(voting);
            _bll.UserInVotings.UpdateUsersInVoting(voting.Id, votingDto.Users);
            _bll.FeatureInVotings.UpdateFeaturesInVoting(voting.Id, votingDto.Features);
            
            try
            {
                await _bll.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _bll.Categories.Exists(votingDto.Id))
                {
                    return NotFound();
                }
                throw;
            }
            return NoContent();
        }
        
        /// <summary>
        /// Add feature to priority voting
        /// </summary>
        /// <param name="featureInVotingDto">Container for voting id and feature id</param>
        /// <returns>Feature in voting object</returns>
        /// <response code="200">Feature was successfully added to voting.</response>
        /// <response code="401">Not authorized to perform action.</response>
        [ProducesResponseType( typeof( FeatureInVotingApiDto ), 200 )]
        [ProducesResponseType( 401 )]
        [Produces( "application/json" )]
        [Consumes("application/json")]
        [HttpPost]
        public async Task<ActionResult<FeatureInVotingApiDto>> AddFeatureToVoting(FeatureInVotingCreateApiDto featureInVotingDto)
        {
            var featureInVoting = _fMapper.MapFeatureInVotingCreate(featureInVotingDto);
            _bll.FeatureInVotings.Add(featureInVoting);
            await _bll.SaveChangesAsync();
            
            return Ok(featureInVoting);
        }
        
        /// <summary>
        /// Add user to voting
        /// </summary>
        /// <param name="userInVotingDto">container for user id and voting id</param>
        /// <returns>User in voting object</returns>
        /// <response code="200">User was successfully added to voting.</response>
        /// <response code="401">Not authorized to perform action.</response>
        [ProducesResponseType( typeof( UserInVotingApiDto ), 200 )]
        [ProducesResponseType( 401 )]
        [Produces( "application/json" )]
        [Consumes("application/json")]
        [HttpPost]
        public async Task<ActionResult<UserInVotingApiDto>> AddUserToVoting(UserInVotingCreateApiDto userInVotingDto)
        {
            var userInVoting = _uMapper.MapUserInVotingCreate(userInVotingDto);
            _bll.UserInVotings.Add(userInVoting);
            await _bll.SaveChangesAsync();
            
            return Ok(userInVoting);
        }
        
        /// <summary>
        /// Create new priority voting, add users and features to voting
        /// </summary>
        /// <param name="votingDto">Voting to create</param>
        /// <returns>Created voting object</returns>
        /// <response code="201">Voting was successfully created.</response>
        /// <response code="401">Not authorized to perform action.</response>
        [ProducesResponseType( typeof( VotingApiDto ), 201 )]
        [ProducesResponseType( 401 )]
        [Produces( "application/json" )]
        [Consumes("application/json")]
        [HttpPost]
        public async Task<ActionResult<VotingApiDto>> CreateVoting(VotingCreateApiDto votingDto)
        {
            var voting = _mapper.MapVotingCreate(votingDto);
            _bll.Votings.Add(voting);
            await _bll.SaveChangesAsync();

            _bll.UserInVotings.UpdateUsersInVoting(voting.Id, votingDto.Users);
            _bll.FeatureInVotings.UpdateFeaturesInVoting(voting.Id, votingDto.Features);
            await _bll.SaveChangesAsync();

            return Ok(voting);
        }

        /// <summary>
        /// Delete voting with specified id
        /// </summary>
        /// <param name="id">Voting id - Guid</param>
        /// <returns>Action result</returns>
        /// <response code="200">Voting was successfully deleted.</response>
        /// <response code="401">Not authorized to perform action.</response>
        /// <response code="404">Voting with specified id was not found.</response>
        [ProducesResponseType( 200)]
        [ProducesResponseType( 401 )]
        [ProducesResponseType( 404 )]
        [Produces( "application/json" )]
        [Consumes("application/json")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVoting(Guid id)
        {
            var voting = await _bll.Votings.FirstOrDefault(id);
            
            if (voting == null)
            {
                return NotFound();
            }

            _bll.Votings.Remove(voting);
            await _bll.SaveChangesAsync();

            return Ok();
        }
        
        /// <summary>
        /// Remove feature from priority voting
        /// </summary>
        /// <param name="featureInVotingDto">Container for voting id and feature id</param>
        /// <returns>Action result</returns>
        /// <response code="200">Feature was successfully removed from voting.</response>
        /// <response code="401">Not authorized to perform action.</response>
        /// <response code="404">Voting or feature with specified id was not found.</response>
        [ProducesResponseType( 200)]
        [ProducesResponseType( 401 )]
        [ProducesResponseType( 404 )]
        [Produces( "application/json" )]
        [Consumes("application/json")]
        [HttpPost]
        public async Task<IActionResult> RemoveFeature(FeatureInVotingCreateApiDto featureInVotingDto)
        {
            var featureInVoting = await _bll.FeatureInVotings.FindFeatureInVoting(featureInVotingDto.FeatureId, featureInVotingDto.VotingId);
            
            if (featureInVoting == null)
            {
                return NotFound();
            }

            _bll.FeatureInVotings.Remove(featureInVoting);
            await _bll.SaveChangesAsync();

            return Ok();
        }
        
        /// <summary>
        /// Remove user from priority voting
        /// </summary>
        /// <param name="userInVotingDto">Container for voting id and user id</param>
        /// <returns>Action result</returns>
        /// <response code="200">User was successfully removed from voting.</response>
        /// <response code="401">Not authorized to perform action.</response>
        /// <response code="404">Voting or user with specified id was not found.</response>
        [ProducesResponseType( 200)]
        [ProducesResponseType( 401 )]
        [ProducesResponseType( 404 )]
        [Produces( "application/json" )]
        [Consumes("application/json")]
        [HttpPost]
        public async Task<IActionResult> RemoveUser(UserInVotingCreateApiDto userInVotingDto)
        {
            var userInVoting = await _bll.UserInVotings.FindUserInVoting(userInVotingDto.AppUserId, userInVotingDto.VotingId);
            
            if (userInVoting == null)
            {
                return NotFound();
            }

            _bll.UserInVotings.Remove(userInVoting);
            await _bll.SaveChangesAsync();

            return Ok();
        }

        /// <summary>
        /// Perform priority voting
        /// Create user priorities and calculate priorities for features for logged in user
        /// </summary>
        /// <param name="userFeaturePriorities">Collection of users feature priories</param>
        /// <returns>Action result</returns>
        /// <response code="201">Voting was successfully performed.</response>
        /// <response code="401">Not authorized to perform action.</response>
        [ProducesResponseType( typeof( VotingApiDto ), 201 )]
        [ProducesResponseType( 401 )]
        [Produces( "application/json" )]
        [Consumes("application/json")]
        [HttpPost]
        public async Task<ActionResult> Vote(IEnumerable<UsersFeaturePriorityCreateApiDto> userFeaturePriorities)
        {
            var dtoList = userFeaturePriorities.ToList();
            if (dtoList.Count == 0)
            {
                return Ok();
            }

            Dictionary<Guid, UsersFeaturePriorityCreateApiDto> withFeatureInVotingId = await _bll.FeatureInVotings.GetFeatureInVotingIds(dtoList);
            _bll.UsersFeaturePriorities.AddUserPriorities(withFeatureInVotingId, User.UserId());
            await _bll.SaveChangesAsync();

            await _bll.FeatureInVotings.CalculatePriorityValuesForVoting(dtoList.First().VotingId);
            await _bll.SaveChangesAsync();
            
            ICollection<Guid> featureIds = dtoList.Select(dto => dto.Id).ToList();
            await _bll.Features.CalculateSizeAndPriority(featureIds);
            await _bll.SaveChangesAsync();

            return Ok();
        }
        
        /// <summary>
        /// Edit priority votes for logged in user
        /// </summary>
        /// <param name="userFeaturePriorities">Collection of users feature priories</param>
        /// <returns>Action result</returns>
        /// <response code="204">Users feature priorities were successfully edited.</response>
        /// <response code="401">Not authorized to perform action.</response>
        /// <response code="400">Bad request.</response>
        /// <response code="404">Feature or voting for specified id was not found.</response>
        [ProducesResponseType( 204)]
        [ProducesResponseType( 401 )]
        [ProducesResponseType( 403 )]
        [ProducesResponseType( 404 )]
        [Produces( "application/json" )]
        [Consumes("application/json")]
        [HttpPost]
        public async Task<ActionResult> EditVotes(IEnumerable<UsersFeaturePriorityCreateApiDto> userFeaturePriorities)
        {
            var dtoList = userFeaturePriorities.ToList();
            if (dtoList.Count == 0)
            {
                return Ok();
            }

            Dictionary<Guid, UsersFeaturePriorityCreateApiDto> withFeatureInVotingId = await _bll.FeatureInVotings.GetFeatureInVotingIds(dtoList);
            _bll.UsersFeaturePriorities.UpdateUserPriorities(withFeatureInVotingId, User.UserId());
            await _bll.SaveChangesAsync();

            await _bll.FeatureInVotings.CalculatePriorityValuesForVoting(dtoList.First().VotingId);
            await _bll.SaveChangesAsync();
            
            ICollection<Guid> featureIds = dtoList.Select(dto => dto.Id).ToList();
            await _bll.Features.CalculateSizeAndPriority(featureIds);
            await _bll.SaveChangesAsync();

            return Ok();
        }
    }
}

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
        public VotingsController(IAppBLL bll)
        {
            _bll = bll;
            _fMapper = new DTOFeatureInVotingMapper();
            _mapper = new DTOVotingMapper();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VotingApiDto>>> GetVotings()
        {
            var votings = (await _bll.Votings.GetAll())
                .Select(bllEntity => _mapper.MapVoting(bllEntity));
            
            return Ok(votings);
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VotingApiDto>>> GetActiveVotings()
        {
            var votings = (await _bll.Votings.GetActiveVotings())
                .Select(bllEntity => _mapper.Map(bllEntity));
            
            return Ok(votings);
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<VotingApiDto>>> GetVotingsForFeature(Guid id)
        {
            var votings = (await _bll.Votings.GetVotingsForFeature(id))
                .Select(bllEntity => _mapper.MapVoting(bllEntity));
            
            return Ok(votings);
        }

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
        
        [HttpPut("{id}")]
        public async Task<IActionResult> EditVoting(Guid id, VotingApiDto votingDto)
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

            voting = _mapper.Map(votingDto);
            _bll.Votings.Edit(voting);
            
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
        
        [HttpPost]
        public async Task<ActionResult<FeatureInVotingApiDto>> AddFeatureToVoting(FeatureInVotingCreateApiDto dto)
        {
            var featureInVoting = _fMapper.MapFeatureInVotingCreate(dto);
            _bll.FeatureInVotings.Add(featureInVoting);
            await _bll.SaveChangesAsync();
            
            return Ok(featureInVoting);
        }
        
        [HttpPost]
        public async Task<ActionResult<VotingApiDto>> CreateVoting(VotingCreateApiDto votingDto)
        {
            var voting = _mapper.MapVotingCreate(votingDto);
            _bll.Votings.Add(voting);
            await _bll.SaveChangesAsync();

            _bll.UserInVotings.AddUsersToVoting(voting.Id, votingDto.Users);
            _bll.FeatureInVotings.AddFeaturesToVoting(voting.Id, votingDto.Features);
            await _bll.SaveChangesAsync();

            return Ok(voting);
        }

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

            return Ok(voting);
        }
        
        [HttpPost]
        public async Task<IActionResult> RemoveFeature(FeatureInVotingCreateApiDto dto)
        {
            var featureInVoting = await _bll.FeatureInVotings.FindFeatureInVoting(dto.FeatureId, dto.VotingId);
            
            if (featureInVoting == null)
            {
                return NotFound();
            }

            _bll.FeatureInVotings.Remove(featureInVoting);
            await _bll.SaveChangesAsync();

            return Ok(dto);
        }
        
        [HttpPost]
        public async Task<IActionResult> RemoveUser(UserInVotingRemoveApiDto dto)
        {
            var userInVoting = await _bll.UserInVotings.FindUserInVoting(dto.AppUserId, dto.VotingId);
            
            if (userInVoting == null)
            {
                return NotFound();
            }

            _bll.UserInVotings.Remove(userInVoting);
            await _bll.SaveChangesAsync();

            return Ok(dto);
        }
    }
}
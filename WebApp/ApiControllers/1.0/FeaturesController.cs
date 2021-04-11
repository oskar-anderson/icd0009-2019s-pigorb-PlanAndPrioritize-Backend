using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public FeaturesController(IAppBLL bll)
        {
            _bll = bll;
            _mapper = new DTOFeatureMapper();
        }

        [HttpGet("{search?}")]
        public ActionResult<IEnumerable<FeatureApiDto>> GetFeaturesForList(string? search)
        {
            var features = _bll.Features.GetAllWithoutCollections(search)
                .Select(bllEntity => _mapper.MapFeature(bllEntity));
            
            return Ok(features);
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FeatureApiDto>>> GetToDoFeatures()
        {
            var features = (await _bll.Features.GetToDoFeatures())
                .Select(bllEntity => _mapper.Map(bllEntity));
            
            return Ok(features);
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<FeatureApiDto>>> GetFeaturesNotInVoting(Guid id)
        {
            var features = (await _bll.Features.GetToDoFeaturesNotInVoting(id))
                .Select(bllEntity => _mapper.MapFeature(bllEntity));
            
            return Ok(features);
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<FeatureApiDto>>> GetFeaturesForVoting(Guid id)
        {
            var features = (await _bll.Features.GetFeaturesForVoting(id))
                .Select(bllEntity => _mapper.MapFeature(bllEntity));
            
            return Ok(features);
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<FeatureApiDto>>> GetFeaturesWithPriority(Guid id)
        {
            var features = (await _bll.Features.GetFeaturesForVoting(id))
                .Select(bllEntity => _mapper.MapFeature(bllEntity));
            
            return Ok(features);
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<FeatureApiDto>> GetFeature(Guid id)
        {
            var featureDto = _mapper.MapFeature(await _bll.Features.FirstOrDefault(id));

            if (featureDto == null)
            {
                return NotFound();
            }

            return Ok(featureDto);
        }
        
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
        
        public ActionResult<IEnumerable<FeatureStatusApiDto>> GetFeatureStates()
        {
            ICollection<FeatureStatusApiDto> states = _bll.Features.GetFeatureStatuses();
            return Ok(states);
        }
    }
}
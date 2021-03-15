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
            var features = (await _bll.Features.GetAll())
                .Select(bllEntity => _mapper.MapFeature(bllEntity));
            
            return Ok(features);
        }
        
        public async Task<ActionResult<IEnumerable<FeatureApiDto>>> GetFeaturesForList()
        {
            var features = (await _bll.Features.GetAllWithoutCollections())
                .Select(bllEntity => _mapper.MapFeature(bllEntity));
            
            return Ok(features);
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<FeatureApiDto>> GetFeature(Guid id)
        {
            var featureDto = _mapper.Map(await _bll.Features.FirstOrDefault(id));

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

            feature = _mapper.MapFeatureEdit(featureDto);
            
            _bll.Features.Edit(feature);
            
            try
            {
                await _bll.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _bll.Features.Exists(id))
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
            _bll.Features.Add(feature);
            await _bll.SaveChangesAsync();

            return CreatedAtAction("GetFeature", new { id = feature.Id, 
                version = HttpContext.GetRequestedApiVersion()?.ToString() ?? "0"}, feature);
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

    }
}
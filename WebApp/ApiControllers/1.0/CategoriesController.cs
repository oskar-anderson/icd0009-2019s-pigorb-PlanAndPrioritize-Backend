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
    public class CategoriesController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly DTOCategoryMapper _mapper;

        public CategoriesController(IAppBLL bll)
        {
            _bll = bll;
            _mapper = new DTOCategoryMapper();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryApiDto>>> GetCategories()
        {
            var categories = (await _bll.Categories.GetCategoriesWithTaskCounts())
                .Select(bllEntity => _mapper.Map(bllEntity));
            
            return Ok(categories);
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryEditApiDto>>> GetCategoriesPlain()
        {
            var categories = (await _bll.Categories.GetAllPlain())
                .Select(bllEntity => _mapper.Map(bllEntity));
            
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryEditApiDto>> GetCategory(Guid id)
        {
            var categoryDto = _mapper.Map(await _bll.Categories.FirstOrDefault(id));

            if (categoryDto == null)
            {
                return NotFound();
            }

            return Ok(categoryDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditCategory(Guid id, CategoryEditApiDto categoryDto)
        {
            if (id != categoryDto.Id)
            {
                return BadRequest();
            }
            
            var category = await _bll.Categories.FirstOrDefault(id);
            if (category == null)
            {
                return BadRequest();
            }

            category = _mapper.MapCategoryEdit(categoryDto);
            _bll.Categories.Edit(category);
            
            try
            {
                await _bll.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _bll.Categories.Exists(categoryDto.Id))
                {
                    return NotFound();
                }
                throw;
            }
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<CategoryApiDto>> CreateCategory(CategoryCreateApiDto categoryDto)
        {
            var category = _mapper.MapCategoryCreate(categoryDto);
            _bll.Categories.Add(category);
            await _bll.SaveChangesAsync();

            return CreatedAtAction("GetCategory", new { id = category.Id, 
                version = HttpContext.GetRequestedApiVersion()?.ToString() ?? "0"}, category);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            var category = await _bll.Categories.FirstOrDefault(id);
            
            if (category == null)
            {
                return NotFound();
            }

            _bll.Categories.Remove(category);
            await _bll.SaveChangesAsync();

            return Ok(category);
        }
    }
}

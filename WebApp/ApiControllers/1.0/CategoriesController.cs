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

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="bll">Application Business Logic Layer Interface</param>
        public CategoriesController(IAppBLL bll)
        {
            _bll = bll;
            _mapper = new DTOCategoryMapper();
        }

        /// <summary>
        /// Get categories with count of all, in progress and closed tasks
        /// </summary>
        /// <returns>Collection of categories</returns>
        /// <response code="200">Collection of categories was successfully retrieved.</response>
        /// <response code="401">Not authorized to see the data.</response>
        [ProducesResponseType(typeof(IEnumerable<CategoryApiDto>), 200)]
        [ProducesResponseType(401)]
        [Produces("application/json")]
        [Consumes("application/json")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryApiDto>>> GetCategories()
        {
            var categories = (await _bll.Categories.GetCategoriesWithTaskCounts())
                .Select(bllEntity => _mapper.Map(bllEntity));

            return Ok(categories);
        }

        /// <summary>
        /// Get categories
        /// </summary>
        /// <returns>Collection of categories</returns>
        /// <response code="200">Collection of categories was successfully retrieved.</response>
        /// <response code="401">Not authorized to see the data.</response>
        [ProducesResponseType(typeof(IEnumerable<CategoryEditApiDto>), 200)]
        [ProducesResponseType(401)]
        [Produces("application/json")]
        [Consumes("application/json")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryEditApiDto>>> GetCategoriesPlain()
        {
            var categories = (await _bll.Categories.GetAllPlain())
                .Select(bllEntity => _mapper.Map(bllEntity));

            return Ok(categories);
        }

        /// <summary>
        /// Get category with specified id
        /// </summary>
        /// <param name="id">Category id - Guid</param>
        /// <returns>Category object</returns>
        /// <response code="200">Category was successfully retrieved.</response>
        /// <response code="401">Not authorized to see the data.</response>
        /// <response code="404">Category for requested id was not found.</response>
        [ProducesResponseType(typeof(CategoryEditApiDto), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [Produces("application/json")]
        [Consumes("application/json")]
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

        /// <summary>
        /// Edit category whit specified id
        /// </summary>
        /// <param name="id">Category id - Guid</param>
        /// <param name="categoryDto">Edited category object</param>
        /// <returns>Action result</returns>
        /// <response code="204">Category was successfully edited.</response>
        /// <response code="401">Not authorized to perform action.</response>
        /// <response code="400">Bad request.</response>
        /// <response code="404">Category for specified id was not found.</response>
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [Produces("application/json")]
        [Consumes("application/json")]
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

        /// <summary>
        /// Create new category
        /// </summary>
        /// <param name="categoryDto">New category object</param>
        /// <returns>Created category object</returns>
        /// <response code="201">Category was successfully created.</response>
        /// <response code="401">Not authorized to perform action.</response>
        [ProducesResponseType(typeof(CategoryCreateApiDto), 201)]
        [ProducesResponseType(401)]
        [Produces("application/json")]
        [Consumes("application/json")]
        [HttpPost]
        public async Task<ActionResult<CategoryApiDto>> CreateCategory(CategoryCreateApiDto categoryDto)
        {
            var category = _mapper.MapCategoryCreate(categoryDto);
            _bll.Categories.Add(category);
            await _bll.SaveChangesAsync();

            return CreatedAtAction("GetCategory", new
            {
                id = category.Id,
                version = HttpContext.GetRequestedApiVersion()?.ToString() ?? "0"
            }, category);
        }

        /// <summary>
        /// Delete category with specified id
        /// </summary>
        /// <param name="id">Category id - Guid</param>
        /// <returns>Action result</returns>
        /// <response code="200">Category was successfully deleted.</response>
        /// <response code="401">Not authorized to perform action.</response>
        /// <response code="404">Category with specified id was not found.</response>
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [Produces("application/json")]
        [Consumes("application/json")]
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

            return Ok();
        }
    }
}
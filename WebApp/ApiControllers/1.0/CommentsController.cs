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

namespace WebApp.ApiControllers._1._0
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CommentsController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly DTOCommentMapper _mapper = new DTOCommentMapper();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="bll">Application Business Logic Layer Interface</param>
        public CommentsController(IAppBLL bll)
        {
            _bll = bll;
        }

        /// <summary>
        /// Get comments added to specific feature
        /// </summary>
        /// <param name="id">Feature id - Guid</param>
        /// <returns>Collection of comments</returns>
        /// <response code="200">Collection of comments was successfully retrieved.</response>
        /// <response code="401">Not authorized to see the data.</response>
        [ProducesResponseType(typeof(IEnumerable<CommentApiDto>), 200)]
        [ProducesResponseType(401)]
        [Produces("application/json")]
        [Consumes("application/json")]
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<CommentApiDto>>> GetCommentsForFeature(Guid id)
        {
            var comments = (await _bll.Comments.GetCommentsForFeature(id))
                .Select(bllEntity => _mapper.MapComment(bllEntity));

            return Ok(comments);
        }

        /// <summary>
        /// Create new comment
        /// </summary>
        /// <param name="commentDto">New comment object</param>
        /// <returns>Created comment object</returns>
        /// <response code="201">Comment was successfully created.</response>
        /// <response code="401">Not authorized to perform action.</response>
        [ProducesResponseType(typeof(CategoryCreateApiDto), 201)]
        [ProducesResponseType(401)]
        [Produces("application/json")]
        [Consumes("application/json")]
        [HttpPost]
        public async Task<ActionResult<CommentApiDto>> CreateComment(CommentCreateApiDto commentDto)
        {
            var comment = _mapper.MapCommentCreate(commentDto);
            comment.AppUserId = User.UserId();
            _bll.Comments.Add(comment);
            await _bll.SaveChangesAsync();

            return Ok();
        }
    }
}
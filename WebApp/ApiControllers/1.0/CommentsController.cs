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

        public CommentsController(IAppBLL bll)
        {
            _bll = bll;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<CommentApiDto>>> GetCommentsForFeature(Guid id)
        {
            var comments = (await _bll.Comments.GetCommentsForFeature(id))
                .Select(bllEntity => _mapper.MapComment(bllEntity));
            
            return Ok(comments);
        }
        
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

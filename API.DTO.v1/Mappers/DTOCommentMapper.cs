using System;
using BLL.App.DTO;

namespace API.DTO.v1.Mappers
{
    public class DTOCommentMapper : DTOAppMapper<CommentBllDto, CommentApiDto>
    {
        public CommentBllDto MapCommentCreate(CommentCreateApiDto commentDto)
        {
            return new CommentBllDto
            {
                Content = commentDto.Content,
                FeatureId = commentDto.FeatureId,
                TimeCreated = DateTime.Now
            };
        }

        public CommentApiDto MapComment(CommentBllDto bllEntity)
        {
            return new CommentApiDto
            {
                Id = bllEntity.Id,
                Content = bllEntity.Content,
                AppUserId = bllEntity.AppUserId,
                FeatureId = bllEntity.AppUserId,
                TimeCreated = bllEntity.TimeCreated,
                User = bllEntity.AppUser?.FirstName + " " + bllEntity.AppUser?.LastName
            };
        }
    }
}
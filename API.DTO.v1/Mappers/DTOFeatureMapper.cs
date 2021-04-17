using System;
using System.Collections.Generic;
using System.Linq;
using BLL.App.DTO;
using Classifiers;

namespace API.DTO.v1.Mappers
{
    public class DTOFeatureMapper : DTOAppMapper<FeatureBllDto, FeatureApiDto>
    {
        public FeatureBllDto MapFeatureCreate(FeatureCreateApiDto apiDTO)
        {
            return Mapper.Map<FeatureCreateApiDto, FeatureBllDto>(apiDTO);
        }
        
        public FeatureBllDto MapFeatureEdit(FeatureEditApiDto apiDTO)
        {
            return Mapper.Map<FeatureEditApiDto, FeatureBllDto>(apiDTO);
        }
        
        public FeatureEditApiDto MapFeatureEdit(FeatureBllDto bllDto)
        {
            return new FeatureEditApiDto
            {
                Id = bllDto.Id,
                FeatureStatus = MapFeatureStatus(bllDto.FeatureStatus),
                Title = bllDto.Title,
                Description = bllDto.Description,
                StartTime = bllDto.StartTime,
                EndTime = bllDto.EndTime,
                CategoryId = bllDto.CategoryId,
                AppUserId = bllDto.AppUserId
            };
        }
        
        public FeatureApiDto MapFeature(FeatureBllDto feature)
        {
            return new FeatureApiDto
            {
                Id = feature.Id,
                Title = feature.Title,
                Size = feature.Size,
                PriorityValue = feature.PriorityValue,
                Description = feature.Description,
                StartTime = feature.StartTime,
                EndTime = feature.EndTime,
                Duration = feature.Duration,
                CategoryId = feature.CategoryId,
                CategoryName = feature.Category!.Title,
                FeatureStatus = MapFeatureStatus(feature.FeatureStatus),
                AppUserId = feature.AppUserId,
                Assignee = feature.AppUser == null ? "" : feature.AppUser.FirstName + " " + feature.AppUser.LastName,
                TimeCreated = feature.TimeCreated,
                CreatedBy = feature.CreatedBy == null ? "" : feature.CreatedBy.FirstName + " " + feature.CreatedBy.LastName,
                LastEdited = feature.LastEdited,
                ChangeLog = feature.ChangeLog ?? "",
                CommentIds = GetCommentIds(feature.Comments),
                VotingIds = GetVotingIds(feature.FeatureInVotings),
                IsInOpenVoting = feature.IsInOpenVoting
            };
        }

        private ICollection<Guid> GetCommentIds(IEnumerable<CommentBllDto>? comments)
        {
            return comments?.Select(comment => comment.Id).ToList() ?? new List<Guid>();
        }
        
        private ICollection<Guid> GetVotingIds(IEnumerable<FeatureInVotingBllDto>? featureInVotings)
        {
            List<VotingBllDto> votings = featureInVotings?.Select(f => f.Voting!).ToList() ?? new List<VotingBllDto>();
            return votings.Select(v => v.Id).ToList();
        }

        private string MapFeatureStatus(FeatureStatus status)
        {
            return status switch
            {
                FeatureStatus.NotStarted => "Not started",
                FeatureStatus.InProgress => "In progress",
                FeatureStatus.InReview => "In review",
                FeatureStatus.ToDeploy => "To deploy",
                FeatureStatus.Closed => "Closed",
                _ => "Incorrect feature status value"
            };
        }
    }
}
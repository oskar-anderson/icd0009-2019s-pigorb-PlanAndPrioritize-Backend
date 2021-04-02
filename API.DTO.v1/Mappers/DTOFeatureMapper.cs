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
                LastEdited = feature.LastEdited,
                ChangeLog = feature.ChangeLog,
                CommentIds = GetCommentIds(feature.Comments),
                VotingIds = GetVotingIds(feature.FeatureInVotings)
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
            switch (status)
            {
                case FeatureStatus.NotStarted:
                    return "Not started";
                case FeatureStatus.InProgress:
                    return "In progress";
                case FeatureStatus.InReview:
                    return "In review";
                case FeatureStatus.ToDeploy:
                    return "To deploy";
                case FeatureStatus.Closed:
                    return "Closed";
                default:
                    return "Incorrect feature status value";
            }
        }
    }
}
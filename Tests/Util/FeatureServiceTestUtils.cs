using System;
using System.Collections.Generic;
using API.DTO.v1;
using BLL.App.DTO;
using Classifiers;
using DAL.App.DTO;

namespace Tests.Util
{
    public static class FeatureServiceTestUtils
    {
        public static FeatureWithUsersPriorityBllDto GetFeatureWithUsersPriority(Guid featureId, Guid votingId, int s,
            int b,
            int t, int r)
        {
            return new FeatureWithUsersPriorityBllDto
            {
                Id = featureId,
                VotingId = votingId,
                Title = "Test task",
                CategoryName = "Test category",
                TaskSize = s,
                BusinessValue = b,
                TimeCriticality = t,
                RiskOrOpportunity = r
            };
        }

        public static UsersFeaturePriorityBllDto GetFeaturePriority(Guid featureInVotingId, Guid featureId,
            Guid categoryId,
            Guid votingId)
        {
            return new UsersFeaturePriorityBllDto
            {
                Id = new Guid(),
                Size = 2,
                BusinessValue = 1,
                TimeCriticality = 2,
                RiskOrOpportunity = 1,
                PriorityValue = 2,
                AppUserId = new Guid(),
                FeatureInVotingId = featureInVotingId,
                FeatureInVoting = new FeatureInVotingBllDto
                {
                    Id = featureInVotingId,
                    FeatureId = featureId,
                    Feature = new FeatureBllDto
                    {
                        Id = featureId,
                        Title = "Test task",
                        CategoryId = categoryId,
                        Category = new CategoryBllDto
                        {
                            Id = categoryId,
                            Title = "Test category"
                        }
                    },
                    VotingId = votingId
                }
            };
        }

        public static FeatureDalDto GetDalFeatureWithVotingId(Guid featureId, Guid votingId, FeatureStatus status)
        {
            return new FeatureDalDto
            {
                Id = featureId,
                Title = "Test task",
                CategoryId = new Guid("00000000-0000-0000-0000-000000000009"),
                Category = new CategoryDalDto
                {
                    Id = new Guid("00000000-0000-0000-0000-000000000009"),
                    Title = "Test category"
                },
                FeatureInVotings = new List<FeatureInVotingDalDto>
                {
                    new FeatureInVotingDalDto
                    {
                        Id = new Guid("00000000-0000-0000-0000-000000000077"),
                        FeatureId = featureId,
                        VotingId = votingId,
                        Voting = new VotingDalDto
                        {
                            Id = votingId,
                            Title = "Test voting",
                            StartTime = DateTime.Parse("April 1, 2021"),
                            EndTime = DateTime.Parse("May 1, 2021")
                        }
                    }
                },
                FeatureStatus = status
            };
        }

        public static FeatureDalDto GetDalFeatureWithVotingStartAndEndTime(Guid featureId, Guid votingId,
            DateTime startDate,
            DateTime endDate)
        {
            return new FeatureDalDto
            {
                Id = featureId,
                Title = "Test task",
                CategoryId = new Guid("00000000-0000-0000-0000-000000000051"),
                FeatureInVotings = new List<FeatureInVotingDalDto>
                {
                    new FeatureInVotingDalDto
                    {
                        Id = new Guid("00000000-0000-0000-0000-000000000031"),
                        FeatureId = featureId,
                        VotingId = votingId,
                        Voting = new VotingDalDto
                        {
                            Id = votingId,
                            Title = "Test voting",
                            StartTime = startDate,
                            EndTime = endDate
                        }
                    }
                }
            };
        }

        public static FeatureDalDto GetDalFeatureWithManyFeatureInVotings(Guid featureId)
        {
            var featureInVotings = new List<FeatureInVotingDalDto>
            {
                GetFeatureInVotingDalDto(featureId, DateTime.Parse("April 1, 2021")),
                GetFeatureInVotingDalDto(featureId, DateTime.MaxValue),
                GetFeatureInVotingDalDto(featureId, DateTime.Parse("March 15, 2020"))
            };

            return new FeatureDalDto
            {
                Id = featureId,
                Title = "Test task",
                CategoryId = new Guid("00000000-0000-0000-0000-000000000011"),
                FeatureInVotings = featureInVotings
            };
        }

        private static FeatureInVotingDalDto GetFeatureInVotingDalDto(Guid featureId, DateTime votingEndTime)
        {
            var votingId = new Guid("00000000-0000-0000-0000-000000000033");

            return new FeatureInVotingDalDto
            {
                Id = new Guid("00000000-0000-0000-0000-000000000044"),
                FeatureId = featureId,
                VotingId = votingId,
                Voting = new VotingDalDto
                {
                    Id = votingId,
                    Title = "Test voting",
                    StartTime = DateTime.Parse("March 1, 2020"),
                    EndTime = votingEndTime
                }
            };
        }

        public static FeatureBllDto GetBllFeature(Guid featureId)
        {
            return new FeatureBllDto
            {
                Id = featureId,
                Title = "Test task",
                CategoryId = new Guid("00000000-0000-0000-0000-000000000051"),
                StartTime = DateTime.Parse("March 1, 2021"),
                EndTime = DateTime.Parse("March 14, 2021"),
                Duration = 13,
                FeatureStatus = FeatureStatus.InProgress,
                TimeCreated = DateTime.Parse("January 1, 2021"),
                LastEdited = DateTime.Parse("January 1, 2021")
            };
        }

        public static FeatureEditApiDto GetApiFeature(Guid featureId)
        {
            return new FeatureEditApiDto
            {
                Id = featureId,
                Title = "Changed task",
                CategoryId = new Guid("00000000-0000-0000-0000-000000000051"),
                StartTime = DateTime.Parse("March 1, 2021"),
                EndTime = DateTime.Parse("March 15, 2021"),
                FeatureStatus = "In progress"
            };
        }

        public static CategoryBllDto GetBllCategory()
        {
            return new CategoryBllDto
            {
                Id = new Guid(),
                Title = "Test category"
            };
        }
    }
}
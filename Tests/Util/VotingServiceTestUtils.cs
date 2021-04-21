using System;
using System.Collections.Generic;
using BLL.App.DTO;
using Classifiers;
using DAL.App.DTO;

namespace Tests.Util
{
    public static class VotingServiceTestUtils
    {
        public static VotingDalDto GetVotingDalDto(Guid votingId, DateTime startTime, DateTime endTime)
        {
            return new VotingDalDto
            {
                Id = votingId,
                Title = "Test voting",
                StartTime = startTime,
                EndTime = endTime,
            };
        }

        public static VotingDalDto GetVotingDalDtoWithFeature(Guid votingId, Guid featureId)
        {
            return new VotingDalDto
            {
                Id = votingId,
                Title = "Test voting",
                StartTime = DateTime.Parse("March 1, 2021"),
                EndTime = DateTime.Parse("March 1, 2051"),
                FeatureInVotings = new List<FeatureInVotingDalDto>
                {
                    new FeatureInVotingDalDto
                    {
                        Id = new Guid(),
                        FeatureId = featureId,
                        VotingId = votingId,
                        Feature = new FeatureDalDto
                        {
                            Id = featureId,
                            Title = "Test title",
                            CategoryId = new Guid()
                        }
                    }
                }
            };
        }

        public static VotingBllDto GetVotingBllDto(Guid votingId, DateTime startTime, DateTime endTime,
            VotingStatus status)
        {
            return new VotingBllDto
            {
                Id = votingId,
                Title = "Test voting",
                StartTime = startTime,
                EndTime = endTime,
                VotingStatus = status
            };
        }

        public static VotingDalDto GetVotingDalDtoWithUserInVotings(Guid votingId, Guid userId)
        {
            return new VotingDalDto
            {
                Id = votingId,
                Title = "Test voting",
                StartTime = DateTime.Parse("March 1, 2021"),
                EndTime = DateTime.Parse("March 1, 2051"),
                UserInVotings = new List<UserInVotingDalDto>
                {
                    new UserInVotingDalDto
                    {
                        Id = new Guid(),
                        VotingId = votingId,
                        AppUserId = userId,
                        AppUser = new AppUserDalDto
                        {
                            Id = new Guid(),
                            FirstName = "Test",
                            LastName = "User",
                            Email = "test@user.com"
                        }
                    }
                }
            };
        }
    }
}
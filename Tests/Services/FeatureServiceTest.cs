using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.App.DTO;
using BLL.App.DTO.Mappers;
using BLL.App.Services;
using Contracts.DAL.App;
using Contracts.DAL.App.Repositories;
using DAL.App.DTO;
using Moq;
using Xunit;

namespace Tests.Services
{
    public class FeatureServiceTest
    {
        private readonly Mock<IFeatureRepository> _repositoryMock;
        private readonly FeatureService _featureService;
        private readonly BLLFeatureMapper _mapper = new ();
        
        public FeatureServiceTest()
        {
            var uowMock = new Mock<IAppUnitOfWork>();
            var uow = uowMock.Object;
            _repositoryMock = new Mock<IFeatureRepository>();
            var repository = _repositoryMock.Object;
            
            uowMock.Setup(u => u.Features).Returns(repository);
            _featureService = new FeatureService(uow);
        }

        [Fact]
        public void TestVotingIsOpenIsInOpenVotingIsTrue()
        {
            var featureId = new Guid("00000000-0000-0000-0000-000000000001");
            var votingId = new Guid("00000000-0000-0000-0000-000000000002");
            var featureDalDto =
                GetDalFeatureWithVotingStartAndEndTime(featureId, votingId, DateTime.Parse("April 1, 2021"), DateTime.MaxValue);
            _repositoryMock.Setup(repo => repo.FirstOrDefault(featureId)).Returns(Task.FromResult(featureDalDto));

            var featureBllDto = _featureService.FirstOrDefault(featureId);
            Assert.True(featureBllDto.Result.IsInOpenVoting, "Should be in open voting");
        }
        
        [Fact]
        public void TestVotingIsClosedIsInOpenVotingIsFalse()
        {
            var featureId = new Guid("00000000-0000-0000-0000-000000000001");
            var votingId = new Guid("00000000-0000-0000-0000-000000000002");
            var featureDalDto =
                GetDalFeatureWithVotingStartAndEndTime(featureId, votingId, DateTime.Parse("April 1, 2021"), DateTime.Parse("April 10, 2021"));
            _repositoryMock.Setup(repo => repo.FirstOrDefault(featureId)).Returns(Task.FromResult(featureDalDto));

            var featureBllDto = _featureService.FirstOrDefault(featureId);
            Assert.False(featureBllDto.Result.IsInOpenVoting, "Should not be in open voting");
        }
        
        [Fact]
        public void TestVotingIsNotYetOpenIsInOpenVotingIsTrue()
        {
            var featureId = new Guid("00000000-0000-0000-0000-000000000001");
            var votingId = new Guid("00000000-0000-0000-0000-000000000002");
            var featureDalDto =
                GetDalFeatureWithVotingStartAndEndTime(featureId, votingId, DateTime.MaxValue, DateTime.MaxValue);
            _repositoryMock.Setup(repo => repo.FirstOrDefault(featureId)).Returns(Task.FromResult(featureDalDto));

            var featureBllDto = _featureService.FirstOrDefault(featureId);
            Assert.True(featureBllDto.Result.IsInOpenVoting, "If voting is assigned but not yet started then this parameter should be true");
        }

        [Fact]
        public void TestIsInManyVotingsLatestIsOpenIsInOpenVotingIsTrue()
        {
            var featureId = new Guid("00000000-0000-0000-0000-000000000001");
            var featureDalDto = GetDalFeatureWithManyFeatureInVotings(featureId);
            _repositoryMock.Setup(repo => repo.FirstOrDefault(featureId)).Returns(Task.FromResult(featureDalDto));
            
            var featureBllDto = _featureService.FirstOrDefault(featureId);
            Assert.True(featureBllDto.Result.IsInOpenVoting, "Should be in open voting because is based on latest voting end date");
        }
        
        [Fact]
        public void TestGetFeaturesForVotingReturnsTwoOfThreeFeatures()
        {
            var featureId1 = new Guid("00000000-0000-0000-0000-000000000001");
            var featureId2 = new Guid("00000000-0000-0000-0000-000000000002");
            var featureId3 = new Guid("00000000-0000-0000-0000-000000000003");
            var votingId1 = new Guid("00000000-0000-0000-0000-000000000004");
            var votingId2 = new Guid("00000000-0000-0000-0000-000000000005");
            
            IEnumerable<FeatureDalDto> features = new List<FeatureDalDto>
            {
                GetDalFeatureWithVotingId(featureId1, votingId1),
                GetDalFeatureWithVotingId(featureId2, votingId1),
                GetDalFeatureWithVotingId(featureId3, votingId2)
            };
            _repositoryMock.Setup(repo => repo.GetAll()).Returns(Task.FromResult(features));
            var expectedFeatureBllDtos = new List<FeatureBllDto>
            {
                _mapper.MapFeature(GetDalFeatureWithVotingId(featureId1, votingId1)),
                _mapper.MapFeature(GetDalFeatureWithVotingId(featureId2, votingId1))
            };
            var actualFeatureBllDtos = _featureService.GetFeaturesForVoting(votingId1).Result.ToList();

            Assert.Equal(2, actualFeatureBllDtos.Count);
            // Assert.Equal(expectedFeatureBllDtos, actualFeatureBllDtos);
        }

        private FeatureDalDto GetDalFeatureWithVotingId(Guid featureId, Guid votingId)
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
                }
            };
        }

        private FeatureDalDto GetDalFeatureWithVotingStartAndEndTime(Guid featureId, Guid votingId, DateTime startDate, DateTime endDate)
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
        
        private FeatureDalDto GetDalFeatureWithManyFeatureInVotings(Guid featureId)
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
        
        private FeatureInVotingDalDto GetFeatureInVotingDalDto(Guid featureId, DateTime votingEndTime)
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
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTO.v1;
using BLL.App.DTO;
using BLL.App.DTO.Mappers;
using BLL.App.Services;
using Classifiers;
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
        private readonly BLLFeatureMapper _mapper = new();

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
                FeatureServiceTestUtils.GetDalFeatureWithVotingStartAndEndTime(featureId, votingId,
                    DateTime.Parse("April 1, 2021"),
                    DateTime.MaxValue);
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
                FeatureServiceTestUtils.GetDalFeatureWithVotingStartAndEndTime(featureId, votingId,
                    DateTime.Parse("April 1, 2021"),
                    DateTime.Parse("April 10, 2021"));
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
                FeatureServiceTestUtils.GetDalFeatureWithVotingStartAndEndTime(featureId, votingId, DateTime.MaxValue,
                    DateTime.MaxValue);
            _repositoryMock.Setup(repo => repo.FirstOrDefault(featureId)).Returns(Task.FromResult(featureDalDto));

            var featureBllDto = _featureService.FirstOrDefault(featureId);
            Assert.True(featureBllDto.Result.IsInOpenVoting,
                "If voting is assigned but not yet started then this parameter should be true");
        }

        [Fact]
        public void TestIsInManyVotingsLatestIsOpenIsInOpenVotingIsTrue()
        {
            var featureId = new Guid("00000000-0000-0000-0000-000000000001");
            var featureDalDto = FeatureServiceTestUtils.GetDalFeatureWithManyFeatureInVotings(featureId);
            _repositoryMock.Setup(repo => repo.FirstOrDefault(featureId)).Returns(Task.FromResult(featureDalDto));

            var featureBllDto = _featureService.FirstOrDefault(featureId);
            Assert.True(featureBllDto.Result.IsInOpenVoting,
                "Should be in open voting because is based on latest voting end date");
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
                FeatureServiceTestUtils.GetDalFeatureWithVotingId(featureId1, votingId1, FeatureStatus.NotStarted),
                FeatureServiceTestUtils.GetDalFeatureWithVotingId(featureId2, votingId1, FeatureStatus.NotStarted),
                FeatureServiceTestUtils.GetDalFeatureWithVotingId(featureId3, votingId2, FeatureStatus.NotStarted)
            };
            _repositoryMock.Setup(repo => repo.GetAll()).Returns(Task.FromResult(features));
            var expectedFeatureBllDtos = new List<FeatureBllDto>
            {
                _mapper.MapFeature(
                    FeatureServiceTestUtils.GetDalFeatureWithVotingId(featureId1, votingId1, FeatureStatus.NotStarted)),
                _mapper.MapFeature(
                    FeatureServiceTestUtils.GetDalFeatureWithVotingId(featureId2, votingId1, FeatureStatus.NotStarted))
            };
            var actualFeatureBllDtos = _featureService.GetFeaturesForVoting(votingId1).Result.ToList();

            Assert.Equal(2, actualFeatureBllDtos.Count);
            Assert.Equal(expectedFeatureBllDtos[0].Id, actualFeatureBllDtos[0].Id);
            Assert.Equal(expectedFeatureBllDtos[1].Id, actualFeatureBllDtos[1].Id);
        }

        [Fact]
        public void TestGetToDoFeaturesNotInVotingReturnsOneOfFour()
        {
            var featureId1 = new Guid("00000000-0000-0000-0000-000000000001");
            var featureId2 = new Guid("00000000-0000-0000-0000-000000000002");
            var featureId3 = new Guid("00000000-0000-0000-0000-000000000003");
            var votingId1 = new Guid("00000000-0000-0000-0000-000000000004");
            var votingId2 = new Guid("00000000-0000-0000-0000-000000000005");

            IEnumerable<FeatureDalDto> features = new List<FeatureDalDto>
            {
                FeatureServiceTestUtils.GetDalFeatureWithVotingId(featureId1, votingId1, FeatureStatus.InReview),
                FeatureServiceTestUtils.GetDalFeatureWithVotingId(featureId2, votingId1, FeatureStatus.NotStarted),
                FeatureServiceTestUtils.GetDalFeatureWithVotingId(featureId3, votingId2, FeatureStatus.NotStarted),
                FeatureServiceTestUtils.GetDalFeatureWithVotingId(featureId3, votingId2, FeatureStatus.InProgress)
            };
            _repositoryMock.Setup(repo => repo.GetAll()).Returns(Task.FromResult(features));
            var expectedFeature =
                _mapper.MapFeature(
                    FeatureServiceTestUtils.GetDalFeatureWithVotingId(featureId2, votingId1, FeatureStatus.NotStarted));

            var actualFeatureBllDtos = _featureService.GetToDoFeaturesNotInVoting(votingId2).Result.ToList();

            Assert.Single(actualFeatureBllDtos);
            Assert.Equal(expectedFeature.Id, actualFeatureBllDtos[0].Id);
        }

        [Fact]
        public void TestGetFeaturesWithUsersPrioritiesReturnsBothFeaturesAlreadyVotedAndNotVoted()
        {
            var featureId1 = new Guid("00000000-0000-0000-0000-000000000001");
            var featureId2 = new Guid("00000000-0000-0000-0000-000000000002");
            var votingId1 = new Guid("00000000-0000-0000-0000-000000000004");
            var featureInVotingId = new Guid("00000000-0000-0000-0000-000000000099");
            var categoryId = new Guid("00000000-0000-0000-0000-000000000077");

            var userPriorities = new List<UsersFeaturePriorityBllDto>
            {
                FeatureServiceTestUtils.GetFeaturePriority(featureInVotingId, featureId1, categoryId, votingId1)
            };
            IEnumerable<FeatureDalDto> allFeatures = new List<FeatureDalDto>
            {
                FeatureServiceTestUtils.GetDalFeatureWithVotingId(featureId1, votingId1, FeatureStatus.NotStarted),
                FeatureServiceTestUtils.GetDalFeatureWithVotingId(featureId2, votingId1, FeatureStatus.NotStarted)
            };
            _repositoryMock.Setup(repo => repo.GetAll()).Returns(Task.FromResult(allFeatures));

            var expectedFeatureBllDtos = new List<FeatureWithUsersPriorityBllDto>
            {
                FeatureServiceTestUtils.GetFeatureWithUsersPriority(featureId1, votingId1, 2, 1, 2, 1),
                FeatureServiceTestUtils.GetFeatureWithUsersPriority(featureId2, votingId1, 0, 0, 0, 0)
            };

            var actualFeatureBllDtos =
                _featureService.GetFeaturesWithUsersPriorities(userPriorities, votingId1).Result.ToList();
            Assert.Equal(expectedFeatureBllDtos, actualFeatureBllDtos);
        }

        [Fact]
        public void TestConstructEditedFeatureWithChangeLog()
        {
            var featureId = new Guid("00000000-0000-0000-0000-000000000001");
            var bllFeature = FeatureServiceTestUtils.GetBllFeature(featureId);
            var apiFeature = FeatureServiceTestUtils.GetApiFeature(featureId);
            var bllCategory = FeatureServiceTestUtils.GetBllCategory();

            var expectedBllFeatureWithChangeLog = new FeatureBllDto
            {
                Id = featureId,
                Title = "Changed task",
                CategoryId = new Guid("00000000-0000-0000-0000-000000000051"),
                StartTime = DateTime.Parse("March 1, 2021"),
                EndTime = DateTime.Parse("March 15, 2021"),
                Duration = 14,
                FeatureStatus = FeatureStatus.InProgress,
                TimeCreated = DateTime.Parse("January 1, 2021"),
                LastEdited = DateTime.Now,
                ChangeLog = "\\n" + DateTime.Now.ToString("dd.MM.yyyy HH:mm") + 
                            " tester changed title from 'Test task' to 'Changed task'." + 
                            "\\n" + DateTime.Now.ToString("dd.MM.yyyy HH:mm") + 
                            " tester changed end date from '14.03.2021 00:00' to '15.03.2021 00:00'." +
                            "\\n" + DateTime.Now.ToString("dd.MM.yyyy HH:mm") + 
                            " tester changed duration from '13' to '14' days."
            };
            var actualBllFeatureWithChangeLog =
                _featureService.ConstructEditedFeatureWithChangeLog(bllFeature, apiFeature, "tester", bllCategory,
                    null);
            Assert.Equal(expectedBllFeatureWithChangeLog.ChangeLog, actualBllFeatureWithChangeLog.ChangeLog);
        }
    }
}
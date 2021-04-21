using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.App.DTO;
using BLL.App.DTO.Mappers;
using BLL.App.Services;
using Classifiers;
using Contracts.DAL.App;
using Contracts.DAL.App.Repositories;
using DAL.App.DTO;
using Moq;
using Tests.Util;
using Xunit;

namespace Tests.Services
{
    public class VotingServiceTest
    {
        private readonly Mock<IVotingRepository> _repositoryMock;
        private readonly VotingService _votingService;
        private readonly BLLVotingMapper _mapper = new();

        public VotingServiceTest()
        {
            var uowMock = new Mock<IAppUnitOfWork>();
            var uow = uowMock.Object;
            _repositoryMock = new Mock<IVotingRepository>();
            var repository = _repositoryMock.Object;

            uowMock.Setup(u => u.Votings).Returns(repository);
            _votingService = new VotingService(uow);
        }

        [Fact]
        public void TestGetVotingsWithVotingStatusAddedBasedOnStartAndEndDate()
        {
            var votingId1 = new Guid("00000000-0000-0000-0000-000000000001");
            var votingId2 = new Guid("00000000-0000-0000-0000-000000000002");
            var votingId3 = new Guid("00000000-0000-0000-0000-000000000003");
            IEnumerable<VotingDalDto> allVotings = new List<VotingDalDto>
            {
                VotingServiceTestUtils.GetVotingDalDto(votingId1, DateTime.Parse("March 1, 2021"),
                    DateTime.Parse("March 10, 2021")),
                VotingServiceTestUtils.GetVotingDalDto(votingId2, DateTime.Parse("March 1, 3000"),
                    DateTime.Parse("March 1, 3001")),
                VotingServiceTestUtils.GetVotingDalDto(votingId3, DateTime.Parse("March 1, 2021"),
                    DateTime.Parse("March 1, 3000"))
            };
            _repositoryMock.Setup(repo => repo.GetAll()).Returns(Task.FromResult(allVotings));

            var expectedVotings = new List<VotingBllDto>
            {
                VotingServiceTestUtils.GetVotingBllDto(votingId1, DateTime.Parse("March 1, 2021"),
                    DateTime.Parse("March 10, 2021"), VotingStatus.Closed),
                VotingServiceTestUtils.GetVotingBllDto(votingId2, DateTime.Parse("March 1, 3000"),
                    DateTime.Parse("March 1, 3001"), VotingStatus.NotOpenYet),
                VotingServiceTestUtils.GetVotingBllDto(votingId3, DateTime.Parse("March 1, 2021"),
                    DateTime.Parse("March 1, 3000"), VotingStatus.Open)
            };

            var actualVotings = _votingService.GetAll().Result.ToList();
            Assert.Equal(expectedVotings, actualVotings);
        }

        [Fact]
        public void TestGetVotingsForFeatureReturnsOneOfTwo()
        {
            var votingId1 = new Guid("00000000-0000-0000-0000-000000000001");
            var votingId2 = new Guid("00000000-0000-0000-0000-000000000002");
            var featureId1 = new Guid("00000000-0000-0000-0000-000000000003");
            var featureId2 = new Guid("00000000-0000-0000-0000-000000000004");
            var voting1 = VotingServiceTestUtils.GetVotingDalDtoWithFeature(votingId1, featureId1);
            var voting2 = VotingServiceTestUtils.GetVotingDalDtoWithFeature(votingId2, featureId2);

            IEnumerable<VotingDalDto> allVotings = new List<VotingDalDto> {voting1, voting2};
            _repositoryMock.Setup(repo => repo.GetAll()).Returns(Task.FromResult(allVotings));

            var expectedVotings = new List<VotingBllDto> {_mapper.MapVoting(voting1)};
            var actualVotings = _votingService.GetVotingsForFeature(featureId1).Result.ToList();

            Assert.Single(actualVotings);
            Assert.Equal(expectedVotings[0].Id, actualVotings[0].Id);
            // Assert Equals can't assert objects with so any nested levels
            Assert.Equal(expectedVotings[0].FeatureInVotings, actualVotings[0].FeatureInVotings);
        }

        [Fact]
        public void TestGetVotingsForFeatureReturnsNone()
        {
            var votingId1 = new Guid("00000000-0000-0000-0000-000000000001");
            var votingId2 = new Guid("00000000-0000-0000-0000-000000000002");
            var featureId1 = new Guid("00000000-0000-0000-0000-000000000003");
            var featureId2 = new Guid("00000000-0000-0000-0000-000000000004");
            var featureId3 = new Guid("00000000-0000-0000-0000-000000000999");
            var voting1 = VotingServiceTestUtils.GetVotingDalDtoWithFeature(votingId1, featureId1);
            var voting2 = VotingServiceTestUtils.GetVotingDalDtoWithFeature(votingId2, featureId2);

            IEnumerable<VotingDalDto> allVotings = new List<VotingDalDto> {voting1, voting2};
            _repositoryMock.Setup(repo => repo.GetAll()).Returns(Task.FromResult(allVotings));

            var actualVotings = _votingService.GetVotingsForFeature(featureId3).Result.ToList();

            Assert.True(actualVotings.Count == 0);
        }

        [Fact]
        public void TestGetActiveVotingsNotInFeatureReturnsOneOfTwo()
        {
            var votingId1 = new Guid("00000000-0000-0000-0000-000000000001");
            var votingId2 = new Guid("00000000-0000-0000-0000-000000000002");
            var featureId1 = new Guid("00000000-0000-0000-0000-000000000003");
            var featureId2 = new Guid("00000000-0000-0000-0000-000000000004");
            var voting1 = VotingServiceTestUtils.GetVotingDalDtoWithFeature(votingId1, featureId1);
            var voting2 = VotingServiceTestUtils.GetVotingDalDtoWithFeature(votingId2, featureId2);

            IEnumerable<VotingDalDto> allVotings = new List<VotingDalDto> {voting1, voting2};
            _repositoryMock.Setup(repo => repo.GetActiveVotingsWithCollections()).Returns(Task.FromResult(allVotings));

            var expectedVotings = new List<VotingBllDto> {_mapper.MapVoting(voting2)};
            var actualVotings = _votingService.GetActiveVotingsNotInFeature(featureId1).Result.ToList();

            Assert.Single(actualVotings);
            Assert.Equal(expectedVotings[0].Id, actualVotings[0].Id);
            // Assert Equals can't assert objects with so any nested levels
            Assert.Equal(expectedVotings[0].FeatureInVotings, actualVotings[0].FeatureInVotings);
        }

        [Fact]
        public void TestGetAssignedVotingsReturnsOneOfThree()
        {
            var votingId1 = new Guid("00000000-0000-0000-0000-000000000001");
            var votingId2 = new Guid("00000000-0000-0000-0000-000000000002");
            var votingId3 = new Guid("00000000-0000-0000-0000-000000000003");
            var userId1 = new Guid("00000000-0000-0000-0000-000000000004");
            var userId2 = new Guid("00000000-0000-0000-0000-000000000005");

            var voting1 = VotingServiceTestUtils.GetVotingDalDtoWithUserInVotings(votingId1, userId1);
            var voting2 = VotingServiceTestUtils.GetVotingDalDtoWithUserInVotings(votingId2, userId2);
            var voting3 = VotingServiceTestUtils.GetVotingDalDtoWithUserInVotings(votingId3, userId1);

            IEnumerable<VotingDalDto> allVotings = new List<VotingDalDto> {voting1, voting2, voting3};
            _repositoryMock.Setup(repo => repo.GetAll()).Returns(Task.FromResult(allVotings));

            var expectedVotings = new List<VotingBllDto> {_mapper.MapVoting(voting2)};
            var actualVotings = _votingService.GetAssignedVotings(userId2).Result.ToList();

            Assert.Single(actualVotings);
            Assert.Equal(expectedVotings[0].Id, actualVotings[0].Id);
            // Assert Equals can't assert objects with so any nested levels
            Assert.Equal(expectedVotings[0].UserInVotings, actualVotings[0].UserInVotings);
        }
    }
}
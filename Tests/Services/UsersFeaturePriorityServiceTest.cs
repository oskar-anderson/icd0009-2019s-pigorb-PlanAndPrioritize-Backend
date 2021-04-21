using System;
using BLL.App.Services;
using Contracts.DAL.App;
using Moq;
using Xunit;

namespace Tests.Services
{
    public class UsersFeaturePriorityServiceTest
    {
        private readonly UsersFeaturePriorityService _usersPriorityService;

        public UsersFeaturePriorityServiceTest()
        {
            var uowMock = new Mock<IAppUnitOfWork>();
            var uow = uowMock.Object;
            _usersPriorityService = new UsersFeaturePriorityService(uow);
        }

        [Fact]
        public void TestCalculatePriorityUsingWsjf()
        {
            const int size = 3;
            const int businessValue = 2;
            const int timeCriticality = 5;
            const int riskOrOpportunity = 3;
            var expectedPriority = Convert.ToDecimal(3.33);
            var actualPriority =
                _usersPriorityService.CalculatePriorityUsingWSJF(size, businessValue, timeCriticality,
                    riskOrOpportunity);
            Assert.Equal(expectedPriority, actualPriority);
        }

        [Fact]
        public void TestCalculatePriorityUsingWsjfWhenSizeIs0()
        {
            const int size = 0;
            const int businessValue = 2;
            const int timeCriticality = 5;
            const int riskOrOpportunity = 3;
            var expectedPriority = Convert.ToDecimal(10);
            var actualPriority =
                _usersPriorityService.CalculatePriorityUsingWSJF(size, businessValue, timeCriticality,
                    riskOrOpportunity);
            Assert.Equal(expectedPriority, actualPriority);
        }
    }
}
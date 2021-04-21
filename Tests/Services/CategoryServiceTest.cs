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
using Xunit;

namespace Tests.Services
{
    public class CategoryServiceTest
    {
        private readonly Mock<ICategoryRepository> _repositoryMock;
        private readonly CategoryService _categoryService;
        private readonly BLLCategoryMapper _mapper = new();

        public CategoryServiceTest()
        {
            var uowMock = new Mock<IAppUnitOfWork>();
            var uow = uowMock.Object;
            _repositoryMock = new Mock<ICategoryRepository>();
            var repository = _repositoryMock.Object;

            uowMock.Setup(u => u.Categories).Returns(repository);
            _categoryService = new CategoryService(uow);
        }

        [Fact]
        public void TestGetCategoriesWithTaskCounts()
        {
            var categoryId1 = new Guid("00000000-0000-0000-0000-000000000001");
            var categoryId2 = new Guid("00000000-0000-0000-0000-000000000002");
            var category1 = GetCategoryWithFeatures(categoryId1);
            var category2 = new CategoryDalDto
            {
                Id = categoryId2,
                Title = "Test category"
            };

            IEnumerable<CategoryDalDto> categories = new List<CategoryDalDto> {category1, category2};
            _repositoryMock.Setup(repo => repo.GetAll()).Returns(Task.FromResult(categories));

            var expectedCategories = new List<CategoryBllDto>
            {
                GetCategoryBllDto(categoryId1, 4, 1, 2),
                GetCategoryBllDto(categoryId2, 0, 0, 0)
            };
            var actualCategories = _categoryService.GetCategoriesWithTaskCounts().Result.ToList();

            Assert.Equal(expectedCategories, actualCategories);
        }

        private CategoryDalDto GetCategoryWithFeatures(Guid categoryId)
        {
            return new CategoryDalDto
            {
                Id = categoryId,
                Title = "Test category",
                Features = new List<FeatureDalDto>
                {
                    GetDalFeature(categoryId, FeatureStatus.InProgress),
                    GetDalFeature(categoryId, FeatureStatus.NotStarted),
                    GetDalFeature(categoryId, FeatureStatus.Closed),
                    GetDalFeature(categoryId, FeatureStatus.Closed)
                }
            };
        }

        private FeatureDalDto GetDalFeature(Guid categoryId, FeatureStatus status)
        {
            return new FeatureDalDto
            {
                Id = new Guid(),
                Title = "Test task",
                CategoryId = categoryId,
                FeatureStatus = status
            };
        }

        private CategoryBllDto GetCategoryBllDto(Guid id, int count, int inProgress, int finished)
        {
            return new CategoryBllDto
            {
                Id = id,
                Title = "Test category",
                Count = count,
                InProgress = inProgress,
                Finished = finished
            };
        }
    }
}
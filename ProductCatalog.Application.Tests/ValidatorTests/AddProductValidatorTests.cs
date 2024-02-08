using FluentAssertions;
using Moq;
using ProductCatalog.Application.Commands;
using ProductCatalog.Domain.Entities;
using ProductCatalog.Domain.Repository;

namespace ProductCatalog.Application.Tests.ValidatorTests
{
    public class AddProductValidatorTests
    {
        private readonly Mock<ICategoryRepository> _mockCategoryRepository;

        public AddProductValidatorTests()
        {
            _mockCategoryRepository = new();
        }

        [Fact]
        public async Task AddProductValidator_Throws_Exception_When_No_Names()
        {
            //Arrange
            _mockCategoryRepository.Setup(c => c.GetByName(It.IsAny<string>())).ReturnsAsync(new CategoryEntity());

            //Act
            var sut = new AddProductValidator(_mockCategoryRepository.Object);

            var result = await sut.ValidateAsync(new AddProductCommand(string.Empty,string.Empty,string.Empty));
            //Assert
            result.Should().NotBeNull();
            result.Errors.Should().HaveCount(3);
        }

        [Fact]
        public async Task AddProductValidator_Throws_Exception_When_Invalid_Category()
        {
            //Arrange
            _mockCategoryRepository.Setup(c => c.GetByName(It.IsAny<string>()));

            //Act
            var sut = new AddProductValidator(_mockCategoryRepository.Object);

            var result = await sut.ValidateAsync(new AddProductCommand("Test", "Test", "Test"));
            
            //Assert
            result.Should().NotBeNull();
            result.Errors.Any(e => e.ErrorMessage == "Invalid category name.").Should().BeTrue();
        }
    }
}

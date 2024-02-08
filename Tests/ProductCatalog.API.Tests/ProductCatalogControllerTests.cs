using FluentAssertions;
using MediatR;
using Moq;
using ProductCatalog.API.Controllers;
using ProductCatalog.Application.Commands;
using ProductCatalog.Application.Queries;
using ProductCatalog.Domain.Entities;

namespace ProductCatalog.API.Tests
{
    public class ProductCatalogControllerTests
    {
        private readonly Mock<IMediator> _mockSender;

        public ProductCatalogControllerTests()
        {
            _mockSender = new();
        }

        [Fact]
        public async Task GetByCategory_Success()
        {
            //Arrange
            _mockSender.Setup(s => s.Send(It.IsAny<GetAllProductsByCategoryQuery>(), default)).ReturnsAsync(new List<ProductEntity>() { new() });
            //Act
            var sut = new ProductCatalogController(_mockSender.Object);
            var result = await sut.GetByCategory(string.Empty);

            //Assert
            result.Should().NotBeEmpty();
            result.Should().BeOfType<List<ProductEntity>>();
        }

        [Fact]
        public async Task Get_Success()
        {
            //Arrange
            _mockSender.Setup(s => s.Send(It.IsAny<GetProductsByIdQuery>(), default)).ReturnsAsync(new ProductEntity() { CategoryName = default });
            //Act
            var sut = new ProductCatalogController(_mockSender.Object);
            var result = await sut.Get(default);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<ProductEntity>();
        }

        [Fact]
        public async Task Post_Success()
        {
            //Arrange
            _mockSender.Setup(s => s.Send(It.IsAny<AddProductCommand>(), default));
            //Act
            var sut = new ProductCatalogController(_mockSender.Object);
            var result = await Record.ExceptionAsync(() => sut.Post(new()));

            //Assert
            result.Should().BeNull();
        }
    }
}
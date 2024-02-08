using AutoMapper;
using FluentAssertions;
using Moq;
using ProductCatalog.Application.Queries;
using ProductCatalog.Domain.Entities;
using ProductCatalog.Domain.Repository;

namespace ProductCatalog.Application.Tests;

public class GetProductsByIdQueryTests
{
    private readonly IMapper _mapper;
    private readonly Mock<IProductCatalogRepository> _mockProductCatalogRepository;

    public GetProductsByIdQueryTests()
    {
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
        _mapper = new Mapper(configuration);
        _mockProductCatalogRepository = new();
    }

    [Fact]
    public async Task GetProductsByIdQueryHandler_Success()
    {
        //Arrange
        _mockProductCatalogRepository.Setup(pc => pc.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new ProductEntity());

        //Act
        var sut = new GetProductsByIdQueryHandler(_mockProductCatalogRepository.Object, _mapper);

        var result = await sut.Handle(new(default), CancellationToken.None);

        //Assert
        result.Should().NotBeNull();
    }
}

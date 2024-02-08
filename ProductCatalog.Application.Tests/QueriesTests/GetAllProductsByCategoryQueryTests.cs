using AutoMapper;
using FluentAssertions;
using Moq;
using ProductCatalog.Application.Queries;
using ProductCatalog.Domain.Entities;
using ProductCatalog.Domain.Repository;

namespace ProductCatalog.Application.Tests;

public class GetAllProductsByCategoryQueryTests
{
    private readonly IMapper _mapper;
    private readonly Mock<IProductCatalogRepository> _mockProductCatalogRepository;

    public GetAllProductsByCategoryQueryTests()
    {
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
        _mapper = new Mapper(configuration);
        _mockProductCatalogRepository = new();
    }

    [Fact]
    public async Task GetAllProductsByCategoryQueryHandler_Success()
    {
        //Arrange
        _mockProductCatalogRepository.Setup(pc => pc.GetByCategoryAsync(It.IsAny<string>())).ReturnsAsync(new List<ProductEntity>() { new() });

        //Act
        var sut = new GetAllProductsByCategoryQueryHandler(_mockProductCatalogRepository.Object,_mapper);

        var result = await sut.Handle(new(string.Empty), CancellationToken.None);

        //Assert
        result.Should().NotBeEmpty();
    }
}

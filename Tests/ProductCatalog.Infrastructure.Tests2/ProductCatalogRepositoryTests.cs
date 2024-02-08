using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using ProductCatalog.Infrastructure.Repository;

namespace ProductCatalog.Infrastructure.Tests;

public class ProductCatalogRepositoryTests
{
    private readonly IMapper _mapper;
    private readonly Mock<ILogger<ProductCatalogRepository>> _logger;
    public ProductCatalogRepositoryTests()
    {
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
        _mapper = new Mapper(configuration);
        _logger = new();
    }

    [Fact]
    public async Task AddAsync_Returns_Success()
    {
        //setup
        using var _dbContext = new  SqliteInMemoryRepositoryTest().CreateContext();

        var sut = new ProductCatalogRepository(_dbContext, _mapper, _logger.Object);

        //Act
        var result = await Record.ExceptionAsync(()=> sut.AddAsync(new()));

        //Assert
        result.Should().BeNull();
    }
}

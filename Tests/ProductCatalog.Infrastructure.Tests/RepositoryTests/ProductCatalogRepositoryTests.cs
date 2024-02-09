using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using ProductCatalog.Domain.Exceptions;
using ProductCatalog.Infrastructure.DBEntities;
using ProductCatalog.Infrastructure.Repository;

namespace ProductCatalog.Infrastructure.Tests;

public class ProductCatalogRepositoryTests
{
    private readonly IMapper _mapper;
    private readonly Mock<ILogger<ProductCatalogRepository>> _logger;
    private readonly IFixture _fixture;

    private const int ItemListCount = 5;

    public ProductCatalogRepositoryTests()
    {
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
        _mapper = new Mapper(configuration);
        _logger = new();

        _fixture = new Fixture().Customize(new AutoMoqCustomization());
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    [Fact]
    public async Task AddAsync_Returns_Success()
    {
        //setup
        using var _dbContext = new SqliteInMemoryRepositoryTest().CreateContext();

        var category = GetSampleCategoryData("Test").First();
        await _dbContext.Categories.AddAsync(category);
        await _dbContext.SaveChangesAsync();

        var sut = new ProductCatalogRepository(_dbContext, _mapper, _logger.Object);

        //Act
        var result = await sut.AddAsync(new() { Name = "Test", Id = Guid.NewGuid(), Code = "Test1", Price = default, CategoryName = category.Name });

        //Assert       
        result.Should().Be(1);
    }

    [Fact]
    public async Task AddAsync_Throws_NoDataFoundException_When_CategoryNotFound()
    {
        var categoryName = "Test";
        //setup
        using var _dbContext = new SqliteInMemoryRepositoryTest().CreateContext();

        var sut = new ProductCatalogRepository(_dbContext, _mapper, _logger.Object);

        //Act
        var result = await Record.ExceptionAsync(() => sut.AddAsync(new() { Name = "Test", Id = Guid.NewGuid(), CategoryName = categoryName }));

        //Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<NoDataFoundException>();
        result.Message.Should().Be($"Category {categoryName} Not found.");
    }

    [Fact]
    public async Task GetAllAsync_Success()
    {
        //setup
        using var _dbContext = new SqliteInMemoryRepositoryTest().CreateContext();

        var category = GetSampleCategoryData("Test").First();
        await _dbContext.Categories.AddAsync(category);
        await _dbContext.SaveChangesAsync();

        var products = GetSampleProducts(category);
        await _dbContext.Products.AddRangeAsync(products);
        await _dbContext.SaveChangesAsync();

        var sut = new ProductCatalogRepository(_dbContext, _mapper, _logger.Object);

        //Act
        var result = await sut.GetAllAsync();

        //Assert       
        result.Should().HaveCount(ItemListCount);
    }

    [Fact]
    public async Task GetByCategoryAsync_Success()
    {
        var categoryName = "Test";
        //setup
        using var _dbContext = new SqliteInMemoryRepositoryTest().CreateContext();

        var category = GetSampleCategoryData(categoryName).First();
        var category2 = GetSampleCategoryData("Test2").First();

        await _dbContext.Categories.AddAsync(category);
        await _dbContext.Categories.AddAsync(category2);
        await _dbContext.SaveChangesAsync();

        var products = GetSampleProducts(category);
        var products2 = GetSampleProducts(category2);

        await _dbContext.Products.AddRangeAsync(products);
        await _dbContext.Products.AddRangeAsync(products2);
        await _dbContext.SaveChangesAsync();

        var sut = new ProductCatalogRepository(_dbContext, _mapper, _logger.Object);

        //Act
        var result = await sut.GetByCategoryAsync(categoryName);

        //Assert       
        result.Should().HaveCount(ItemListCount);
        result.All(r => r.CategoryName == categoryName).Should().BeTrue();
    }

    [Fact]
    public async Task GetByCategoryAsync_Throws_NoDataFoundException()
    {
        var categoryName = "Test";
        //setup
        using var _dbContext = new SqliteInMemoryRepositoryTest().CreateContext();


        var sut = new ProductCatalogRepository(_dbContext, _mapper, _logger.Object);

        //Act
        var result = await Record.ExceptionAsync(() => sut.GetByCategoryAsync(categoryName));

        //Assert       
        result.Should().NotBeNull();
        result.Should().BeOfType<NoDataFoundException>();
    }

    [Fact]
    public async Task GetByIdAsync_Success()
    {
        //setup
        using var _dbContext = new SqliteInMemoryRepositoryTest().CreateContext();

        var category = GetSampleCategoryData().First();
        await _dbContext.Categories.AddAsync(category);
        await _dbContext.SaveChangesAsync();

        var products = GetSampleProducts(category);
        await _dbContext.Products.AddRangeAsync(products);
        await _dbContext.SaveChangesAsync();

        var sut = new ProductCatalogRepository(_dbContext, _mapper, _logger.Object);

        //Act
        var result = await sut.GetByIdAsync(products.First().Id);

        //Assert       
        result.Should().NotBeNull();
    }

    private IEnumerable<Category> GetSampleCategoryData(string? name = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            _fixture.Build<Category>()
                      .CreateMany(ItemListCount);

        return _fixture.Build<Category>()
                      .With(c => c.Name, name)
                      .CreateMany(ItemListCount);
    }

    private IEnumerable<Product> GetSampleProducts(Category category)
    {
        return _fixture.Build<Product>()
                      .With(c => c.Category, category)
                      .CreateMany(ItemListCount);
    }
}

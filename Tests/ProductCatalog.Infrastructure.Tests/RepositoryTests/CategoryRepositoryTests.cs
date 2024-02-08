using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using ProductCatalog.Domain.Entities;
using ProductCatalog.Domain.Exceptions;
using ProductCatalog.Infrastructure.DBEntities;
using ProductCatalog.Infrastructure.Repository;


namespace ProductCatalog.Infrastructure.Tests.RepositoryTests;

public class CategoryRepositoryTests
{
    private readonly IMapper _mapper;
    private readonly Mock<ILogger<CategoryRepository>> _logger;
    private readonly IFixture _fixture;

    private const int ItemListCount = 5;

    public CategoryRepositoryTests()
    {
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
        _mapper = new Mapper(configuration);
        _logger = new();

        _fixture = new Fixture().Customize(new AutoMoqCustomization());
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    [Fact]
    public async Task GetAll_Success()
    {
        //Arrage
        var categories = GetSampleCategoryData();

        using var _dbContext = new SqliteInMemoryRepositoryTest().CreateContext();

        var seededRecords = await _dbContext.Categories.CountAsync();

        await _dbContext.AddRangeAsync(categories);
        await _dbContext.SaveChangesAsync();
       

        //Act
        var sut = new CategoryRepository(_dbContext, _mapper, _logger.Object);

        var result = await sut.GetAll();

        //Assert
        result.Should().HaveCount(ItemListCount + seededRecords);
    }

    [Fact]
    public async Task GetByName_Success()
    {
        var categoryName = "Test";
        //Arrage
        var categories = GetSampleCategoryData(categoryName);

        using var _dbContext = new SqliteInMemoryRepositoryTest().CreateContext();

        await _dbContext.AddAsync(categories.First());
        await _dbContext.SaveChangesAsync();

        //Act
        var sut = new CategoryRepository(_dbContext, _mapper, _logger.Object);

        var result = await sut.GetByName(categoryName);

        //Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<CategoryEntity>();
        result.Name.Should().Be(categoryName);
    }

    [Fact]
    public async Task GetByName_Throws_NoDataFoundException()
    {
        var categoryName = "Test";
        //Arrage  
        using var _dbContext = new SqliteInMemoryRepositoryTest().CreateContext();      

        //Act
        var sut = new CategoryRepository(_dbContext, _mapper, _logger.Object);

        var result = await Record.ExceptionAsync(()=> sut.GetByName(categoryName));

        //Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<NoDataFoundException>();
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
}

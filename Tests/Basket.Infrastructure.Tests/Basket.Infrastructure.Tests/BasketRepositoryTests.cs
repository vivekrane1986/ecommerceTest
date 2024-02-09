using AutoFixture;
using AutoFixture.AutoMoq;
using Basket.Domain.Entities;
using Basket.Infrastrcuture.Repository;
using FluentAssertions;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using System.Text;
using System.Text.Json;

namespace Basket.Infrastructure.Tests;

public class BasketRepositoryTests
{
    private readonly Mock<IDistributedCache> _mockCache;
    private readonly IFixture _fixture;

    public BasketRepositoryTests()
    {
        _mockCache = new();

        _fixture = new Fixture().Customize(new AutoMoqCustomization());
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    [Fact]
    public async Task AddItemAsync_Success()
    {
        //Arrange
        _mockCache.Setup(c => c.SetAsync(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<DistributedCacheEntryOptions>(), It.IsAny<CancellationToken>()));

        var sut = new BasketRepository(_mockCache.Object);
        //Act

        var result = await sut.AddItemAsync(new());

        //Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task GetAllBasketItemsAsync_Success()
    {
        //Arrange
        var orderId = Guid.NewGuid();
        var serializedData = JsonSerializer.Serialize(_fixture.Build<BasketEntity>().With(b => b.OrderId, orderId).CreateMany(5));
        var dataAsByteArray = Encoding.UTF8.GetBytes(serializedData);

        _mockCache.Setup(c => c.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(dataAsByteArray);

        var sut = new BasketRepository(_mockCache.Object);
        //Act

        var result = await sut.GetAllBasketItemsAsync(orderId);

        //Assert
        result.Should().HaveCount(5);
    }

    [Theory]
    [InlineData("")]
    public async Task GetAllBasketItemsAsync_Return_Null_When_NoData(string? data)
    {
        //Arrange
        var orderId = Guid.NewGuid();

        var dataAsByteArray = Encoding.UTF8.GetBytes(data);

        _mockCache.Setup(c => c.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(dataAsByteArray);

        var sut = new BasketRepository(_mockCache.Object);
        //Act

        var result = await sut.GetAllBasketItemsAsync(orderId);

        //Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task RemoveItemAsync_Success()
    {
        //Arrange
        _mockCache.Setup(c => c.RemoveAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()));

        var sut = new BasketRepository(_mockCache.Object);
        //Act

        var result = await sut.RemoveItemAsync(new());

        //Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task SaveBasketDetailsAsync_Success()
    {
        //Arrange
        var sut = new BasketRepository(_mockCache.Object);

        //Act
        var result = await sut.SaveBasketDetailsAsync(new());

        //Assert
        result.Should().BeTrue();
    }
}
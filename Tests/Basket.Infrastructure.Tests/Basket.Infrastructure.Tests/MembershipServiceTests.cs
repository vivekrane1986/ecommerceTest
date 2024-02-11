using Basket.Infrastrcuture;
using Basket.Infrastrcuture.Services;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using System.Net;

namespace Basket.Infrastructure.Tests
{
    public class MembershipServiceTests
    {
        string mockUri = "http://Test/";

        private readonly Mock<IHttpClientFactory> _mockHttpClientFactory;
        private readonly IConfiguration _mockConfig;
        private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;

        public MembershipServiceTests()
        {
            _mockHttpClientFactory = new();

            var config = new Dictionary<string, string>() { { "Membership:Endpoint", "Test/" } };

            _mockConfig = new ConfigurationBuilder().AddInMemoryCollection(config).Build();
            _mockHttpMessageHandler = new();
        }

        [Fact]
        public async Task GetMembershipDiscountAsync_Success()
        {
            //Arrange
            var response = new HttpResponseMessage() { Content = new StringContent("10"), StatusCode = HttpStatusCode.OK };

            _mockHttpClientFactory.Setup(x => x.CreateClient(HttpClientConsts.MembershipDataHttpClient)).Returns(new HttpClient(_mockHttpMessageHandler.Object) { BaseAddress = new Uri(mockUri) });

            _mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>
                ("SendAsync", ItExpr.Is<HttpRequestMessage>(x => x.RequestUri == new Uri($"{mockUri}Test/")), ItExpr.IsAny<CancellationToken>()).ReturnsAsync(response);

            //Act
            var sut = new MembershipService(_mockHttpClientFactory.Object, _mockConfig);

            var result = await sut.GetMembershipDiscountAsync(string.Empty);

            //Assert
            result.Should().Be(10);

        }

        [Fact]
        public async Task GetMembershipDiscountAsync_Throws_Exception_When_Failed()
        {
            //Arrange
            var response = new HttpResponseMessage() { Content = new StringContent("Error"), StatusCode = HttpStatusCode.InternalServerError };

            _mockHttpClientFactory.Setup(x => x.CreateClient(HttpClientConsts.MembershipDataHttpClient)).Returns(new HttpClient(_mockHttpMessageHandler.Object) { BaseAddress = new Uri(mockUri) });

            _mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>
                ("SendAsync", ItExpr.Is<HttpRequestMessage>(x => x.RequestUri == new Uri($"{mockUri}Test/")), ItExpr.IsAny<CancellationToken>()).ReturnsAsync(response);

            //Act
            var sut = new MembershipService(_mockHttpClientFactory.Object, _mockConfig);

            var result = await Record.ExceptionAsync(()=>sut.GetMembershipDiscountAsync(string.Empty));

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<Exception>();
            result.Message.Should().Be("Error Calling Membership API statuscode InternalServerError.");

        }
    }
}

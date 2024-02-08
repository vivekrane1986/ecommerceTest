using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using ProductCatalog.API.Middleware;

namespace ProductCatalog.API.Tests;

public class ExceptionHandlingMiddlewareTests
{

    private readonly Mock<RequestDelegate> _mockNext;
    private readonly Mock<ILogger<ExceptionHandlingMiddleware>> _mockLogger;

    public ExceptionHandlingMiddlewareTests()
    {
        _mockNext = new();
        _mockLogger = new();
    }

    [Fact]
    public async Task InvokeAsync_Success()
    {
        //Arrange
        _mockNext.Setup(n => n(It.IsAny<HttpContext>()));

        //Act
        var sut = new ExceptionHandlingMiddleware(_mockLogger.Object, _mockNext.Object);
        var httpContext = new DefaultHttpContext();
        await sut.InvokeAsync(httpContext);

        //Assert
        httpContext.Response.StatusCode.Should().Be(StatusCodes.Status200OK);

    }


    [Fact]
    public async Task InvokeAsync_Returns_BadRequest()
    {
        //Arrange
        _mockNext.Setup(n => n(It.IsAny<HttpContext>())).ThrowsAsync(new ValidationException(new List<ValidationFailure>() { new ValidationFailure("Test", "Test") }));

        //Act
        var sut = new ExceptionHandlingMiddleware(_mockLogger.Object, _mockNext.Object);
        var httpContext = new DefaultHttpContext();
        await sut.InvokeAsync(httpContext);

        //Assert
        httpContext.Response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);

    }

    [Fact]
    public async Task InvokeAsync_Returns_InternalServerError()
    {
        //Arrange
        _mockNext.Setup(n => n(It.IsAny<HttpContext>())).ThrowsAsync(new Exception());

        //Act
        var sut = new ExceptionHandlingMiddleware(_mockLogger.Object, _mockNext.Object);
        var httpContext = new DefaultHttpContext();
        await sut.InvokeAsync(httpContext);

        //Assert
        httpContext.Response.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);

    }

}

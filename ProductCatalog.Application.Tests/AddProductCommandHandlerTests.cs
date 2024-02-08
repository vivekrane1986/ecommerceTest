using AutoFixture;
using AutoFixture.AutoMoq;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using ProductCatalog.Application.Commands;
using ProductCatalog.Domain.Entities;
using ProductCatalog.Domain.Repository;


namespace ProductCatalog.Application.Tests;

public class AddProductCommandHandlerTests
{
    private readonly Mock<IProductCatalogRepository> _mockProductCatalogRepository;
    private readonly Mock<IValidator<AddProductCommand>> _mockValidator;

    private readonly IFixture _fixture;
    

    public AddProductCommandHandlerTests()
    {
        _mockProductCatalogRepository = new();
        _mockValidator = new();

        _fixture = new Fixture().Customize(new AutoMoqCustomization());
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    [Fact]
    public async Task AddProductCommandHandler_Handle_Success()
    {
        //Arrange
        _mockProductCatalogRepository.Setup(pc => pc.AddAsync(It.IsAny<ProductEntity>()));
        //_mockValidator.Setup(v => v.ValidateAsync(It.IsAny<AddProductCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(new ValidationResult());

        _mockValidator.Setup(p => p.ValidateAsync(It.Is<ValidationContext<AddProductCommand>>(context => !context.ThrowOnFailures), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_fixture.Build<ValidationResult>().Create());

        //Act
        var sut = new AddProductCommandHandler(_mockProductCatalogRepository.Object, _mockValidator.Object);

        var result = await Record.ExceptionAsync(() => sut.Handle(new(string.Empty, string.Empty, string.Empty), CancellationToken.None));

        //Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task AddProductCommandHandler_Throws_ValidationException()
    {
        //Arrange
        _mockProductCatalogRepository.Setup(pc => pc.AddAsync(It.IsAny<ProductEntity>()));
        var validationResult = _fixture.Build<ValidationResult>().Create();

        _mockValidator.Setup(p => p.ValidateAsync(It.Is<ValidationContext<AddProductCommand>>(context => context.ThrowOnFailures), It.IsAny<CancellationToken>()))
                    .Throws(new FluentValidation.ValidationException(validationResult.Errors));


        //Act
        var sut = new AddProductCommandHandler(_mockProductCatalogRepository.Object, _mockValidator.Object);

        var result = await Record.ExceptionAsync(() => sut.Handle(new(string.Empty, string.Empty, string.Empty), CancellationToken.None));

        //Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<ValidationException>();
    }
}
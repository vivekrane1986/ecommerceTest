using AutoMapper;
using FluentAssertions;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using ProductCatalog.Application.Commands;
using ProductCatalog.Infrastructure;

namespace ProductCatalog.Application.Tests;

public class DepedendencyInjectionTests
{


    [Fact]
    public void AddApplication_Success()
    {
        //Arrange
        var services = new ServiceCollection();
                
        //Act
        services.AddApplication();
        services.AddInfrastructure();
        services.AddLogging();

        var serviceProvider = services.BuildServiceProvider();

        var validator = serviceProvider.GetService<IValidator<AddProductCommand>>();
        var mapper = serviceProvider.GetService<IMapper>();
        var mediator = serviceProvider.GetService<IMediator>();
        
        //Assert
        validator.Should().NotBeNull();
        mapper.Should().NotBeNull();
        mediator.Should().NotBeNull();
    }
}

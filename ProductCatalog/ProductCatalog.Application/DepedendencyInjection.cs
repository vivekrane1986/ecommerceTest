using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace ProductCatalog.Application;

public static class DepedendencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services )
    {
        var assembly = typeof(DepedendencyInjection).Assembly;

        services.AddMediatR(configuration => 
            configuration.RegisterServicesFromAssembly(assembly));
        
        services.AddValidatorsFromAssembly(assembly);

        var mapperConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new MappingProfile());
        });

        IMapper mapper = mapperConfig.CreateMapper();

        return services;
    }
}

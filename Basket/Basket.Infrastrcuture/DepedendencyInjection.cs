using Basket.Domain;
using Basket.Infrastrcuture.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Basket.Infrastrcuture;

[ExcludeFromCodeCoverage(Justification = "Sample code so not covering all DIs")]
public static class DepedendencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = config.GetConnectionString("RedisConnectionString");
            options.InstanceName = "EasyGroceryBasket";
        });

        services.AddScoped<IBasketRepository, BasketRepository>();

        return services;
    }

}

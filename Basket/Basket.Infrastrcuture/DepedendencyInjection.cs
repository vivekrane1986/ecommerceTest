using Basket.Domain;
using Basket.Domain.Services;
using Basket.Infrastrcuture.Repository;
using Basket.Infrastrcuture.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly.Extensions.Http;
using Polly;
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
        services.AddHttpClient<IMembershipService, MembershipService>(client =>
        {
            client.BaseAddress = new Uri(config["Membership.BaseUrl"]);
        }).AddPolicyHandler(GetRetryPolicy());

        return services;
    }

    static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
            .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2,
                                                                        retryAttempt)));
    }
}

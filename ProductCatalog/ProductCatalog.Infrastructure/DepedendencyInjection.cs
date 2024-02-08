using AutoMapper;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProductCatalog.Domain.Repository;
using ProductCatalog.Infrastructure.Persistance;
using ProductCatalog.Infrastructure.Repository;
using System.Data.Common;

namespace ProductCatalog.Infrastructure;

public static class DepedendencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<DbConnection, SqliteConnection>(serviceProvider =>
        {
            var connection = new SqliteConnection("Data Source=:memory:");
            connection.Open();
            return connection;
        });

        services.AddDbContext<EasyCommerceDbContext>((serviceProvider, options) =>
        {
            var connection = serviceProvider.GetRequiredService<DbConnection>();
            options.UseSqlite(connection);
        });

        var provider = services
            .BuildServiceProvider()
            .UpdateDatabase();

        var mapperConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new MappingProfile());
        });

        IMapper mapper = mapperConfig.CreateMapper();
        services.AddSingleton(mapper);

        services.AddScoped<IProductCatalogRepository, ProductCatalogRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();

        return services;
    }

    public static IServiceProvider UpdateDatabase(this IServiceProvider provider)
    {
        using (var scope = provider.GetRequiredService<IServiceScopeFactory>().CreateScope())
        using (var context = scope.ServiceProvider.GetService<EasyCommerceDbContext>())
            context.Database.EnsureCreated();

        return provider;
    }
}

using ProductCatalog.API.Middleware;
using ProductCatalog.Application;
using ProductCatalog.Infrastructure;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

[ExcludeFromCodeCoverage(Justification = "This class will be tested as a part of Integration tests")]

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        AddDependencies(builder.Services);

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseMiddleware<ExceptionHandlingMiddleware>();

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }

    private static void AddDependencies(IServiceCollection services)
    {
        services.AddApplication();
        services.AddInfrastructure();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
    }
}
using ProductCatalog.API.Middleware;
using ProductCatalog.Application;
using ProductCatalog.Infrastructure;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Azure.Identity;

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
        builder.Services.AddApplicationInsightsTelemetry();
        

        AddKeyVault(builder.Configuration);
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

    private static void AddKeyVault(ConfigurationManager config)
    {
        var credential = new DefaultAzureCredential(new DefaultAzureCredentialOptions { ExcludeEnvironmentCredential = true });

        config.AddAzureKeyVault(new Uri($"https://{config["KeyVaultName"]}.vault.azure.net/"), credential);
    }
}
using System.Reflection;
using Basket.Infrastrcuture;
using Basket.Application;
using System.Diagnostics.CodeAnalysis;
using Azure.Identity;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;

[ExcludeFromCodeCoverage(Justification = "Sample code - not covering all Classes")]
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
        AddKeyVault(builder.Configuration);
               
        AddDependencies(builder.Services, builder.Configuration);

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }

    private static void AddDependencies(IServiceCollection services, IConfiguration configuration)
    {
        services.AddApplication();
        services.AddInfrastructure(configuration);
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
    }

    private static void AddKeyVault(ConfigurationManager config)
    {
        var credential = new DefaultAzureCredential(new DefaultAzureCredentialOptions {ExcludeEnvironmentCredential=true });

        config.AddAzureKeyVault(new Uri($"https://{config["KeyVaultName"]}.vault.azure.net/"), credential);
    }
}
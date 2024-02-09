
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System.Diagnostics.CodeAnalysis;

namespace ProductCatalog.Infrastructure;

[ExcludeFromCodeCoverage(Justification = "Sample code so not covering all Classes")]
public  class AzureRedisConnection
{
    private  readonly IConfiguration _config;
    private readonly Lazy<ConnectionMultiplexer> _lazyConnection;

    public  AzureRedisConnection(IConfiguration config)
    {
        _config = config;
        _lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
        {
            string redisCacheConnection = _config["RedisCacheSecretKey"];
            return ConnectionMultiplexer.Connect(redisCacheConnection);
        });
    }
       
    public  ConnectionMultiplexer Connection
    {
        get
        {
            return _lazyConnection.Value;
        }
    }
}

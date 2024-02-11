using Basket.Domain.Services;
using Microsoft.Azure.ServiceBus;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.Json;

namespace Basket.Infrastrcuture.Services;

[ExcludeFromCodeCoverage(Justification = "Sample code - not covering all Classes")]
public class MessagePublisher : IMessagePublisher
{
    private readonly IQueueClient _queueClient;

    public MessagePublisher(IQueueClient queueClient)
    {
        _queueClient = queueClient;
    }

    public Task PublishAsync<T>(T obj) where T : class
    {
        var objText = JsonSerializer.Serialize(obj);

        return _queueClient.SendAsync(new Message() { Body = Encoding.UTF8.GetBytes(objText) });
    }
}

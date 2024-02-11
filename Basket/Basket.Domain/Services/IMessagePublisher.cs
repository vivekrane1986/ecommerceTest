namespace Basket.Domain.Services;

public interface IMessagePublisher
{
    Task PublishAsync<T>(T obj) where T : class;
}

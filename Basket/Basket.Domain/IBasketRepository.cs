using Basket.Domain.Entities;

namespace Basket.Domain;

public interface IBasketRepository
{
    Task<bool> AddItemAsync(BasketEntity entity);

    Task<bool> RemoveItemAsync(BasketEntity entity);

    Task<List<BasketEntity>> GetAllBasketItemsAsync(Guid orderId);

    Task<bool> SaveBasketDetailsAsync(List<BasketEntity> basketEntities);
}

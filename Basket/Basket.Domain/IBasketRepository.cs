using Basket.Domain.Entities;

namespace Basket.Domain;

public interface IBasketRepository
{
    Task<string> AddItemAsync(BasketEntity entity);

    Task<bool> RemoveItemAsync(BasketEntity entity);

    Task<List<BasketEntity>> GetAllBasketItemsAsync(string CustomerId);

    Task<bool> SaveBasketDetailsAsync(List<BasketEntity> basketEntities);
}

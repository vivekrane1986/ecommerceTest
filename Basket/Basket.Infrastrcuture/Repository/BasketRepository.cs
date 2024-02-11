using Basket.Domain;
using Basket.Domain.Entities;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;
using System.Text.Json;

namespace Basket.Infrastrcuture.Repository
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache _cache;

        public BasketRepository(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<string> AddItemAsync(BasketEntity entity)
        {
            var customerId= Guid.NewGuid().ToString();
            if (entity.CustomerId is null)
            {
                entity.CustomerId = customerId;
            }

            var serializedData = JsonSerializer.Serialize(entity);
            var dataAsByteArray = Encoding.UTF8.GetBytes(serializedData);
            await _cache.SetAsync(entity.CustomerId, dataAsByteArray);

            return customerId;
        }

        public async Task<List<BasketEntity>> GetAllBasketItemsAsync(string customerId)
        {
            var dataAsByteArray = await _cache.GetAsync(customerId);

            if ((dataAsByteArray.Count()) > 0)
            {
                var serializedData = Encoding.UTF8.GetString(dataAsByteArray);

                return JsonSerializer.Deserialize
                    <List<BasketEntity>>(serializedData);
            }

            return null;
        }

        public async Task<bool> RemoveItemAsync(BasketEntity entity)
        {
            await _cache.RemoveAsync(entity.CustomerId.ToString());

            return true;
        }

        public async Task<bool> SaveBasketDetailsAsync(List<BasketEntity> basketEntities)
        {
           
            await Task.FromResult(basketEntities);

            return true;
        }
    }
}

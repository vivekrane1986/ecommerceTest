using ProductCatalog.Domain.Entities;

namespace ProductCatalog.Domain.Repository;

public interface IProductCatalogRepository
{
    Task<ProductEntity> GetByIdAsync(Guid Id);

    Task<IEnumerable<ProductEntity>> GetAllAsync();

    Task<IEnumerable<ProductEntity>> GetByCategoryAsync(string categoryName);

    Task<int> AddAsync(ProductEntity product);
}

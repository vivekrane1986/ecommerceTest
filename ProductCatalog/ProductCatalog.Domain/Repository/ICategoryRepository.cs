using ProductCatalog.Domain.Entities;

namespace ProductCatalog.Domain.Repository;

public interface ICategoryRepository
{
    Task<CategoryEntity> GetByName(string name);

    Task<IEnumerable<CategoryEntity>> GetAll();
}

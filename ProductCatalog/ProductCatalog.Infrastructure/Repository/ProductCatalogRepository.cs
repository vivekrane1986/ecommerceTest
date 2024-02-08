using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProductCatalog.Domain.Entities;
using ProductCatalog.Domain.Exceptions;
using ProductCatalog.Domain.Repository;
using ProductCatalog.Infrastructure.DBEntities;
using ProductCatalog.Infrastructure.Persistance;

namespace ProductCatalog.Infrastructure.Repository;

public class ProductCatalogRepository : IProductCatalogRepository
{
    private readonly EasyCommerceDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly ILogger<ProductCatalogRepository> _logger;

    public ProductCatalogRepository(EasyCommerceDbContext dbContext, IMapper mapper, ILogger<ProductCatalogRepository> logger)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<int> AddAsync(ProductEntity product)
    {
        try
        {
            var category = await _dbContext.Categories.FirstOrDefaultAsync(c => c.Name == product.CategoryName);

            if (category is null)
            {
                throw new NoDataFoundException($"Category {product.CategoryName} Not found.");
            }

            var productEntity = _mapper.Map<Product>(product);
            productEntity.Category = category;

            await _dbContext.Products.AddAsync(productEntity);
            return await _dbContext.SaveChangesAsync();

        }
        catch (Exception ex)
        {
            _logger.LogError($"Error saving product {ex.Message}");
            throw;
        }
    }

    public async Task<IEnumerable<ProductEntity>> GetAllAsync()
    {
        return _mapper.Map<IEnumerable<ProductEntity>>(await _dbContext.Products.ToListAsync());
    }

    public async Task<IEnumerable<ProductEntity>> GetByCategoryAsync(string categoryName)
    {
        var category = await _dbContext.Categories
            .Include(p => p.Products)
            .Where(p => p.Name == categoryName)
            .FirstOrDefaultAsync();

        return _mapper.Map<IEnumerable<ProductEntity>>(category.Products);

    }

    public async Task<ProductEntity> GetByIdAsync(Guid Id)
    {
        var product = await _dbContext.Products
            .Where(p => p.Id == Id)
            .FirstOrDefaultAsync();

        return _mapper.Map<ProductEntity>(product);
    }
}

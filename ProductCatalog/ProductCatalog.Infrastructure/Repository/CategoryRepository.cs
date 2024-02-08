using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProductCatalog.Domain.Entities;
using ProductCatalog.Domain.Exceptions;
using ProductCatalog.Domain.Repository;
using ProductCatalog.Infrastructure.Persistance;


namespace ProductCatalog.Infrastructure.Repository;

public class CategoryRepository : ICategoryRepository
{
    private readonly EasyCommerceDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly ILogger<CategoryRepository> _logger;


    public CategoryRepository(EasyCommerceDbContext dbContext, IMapper mapper, ILogger<CategoryRepository> logger)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _logger = logger;
    }


    public async Task<IEnumerable<CategoryEntity>> GetAll()
    {
        var categories = await _dbContext.Categories.ToListAsync();
        return _mapper.Map<IEnumerable<CategoryEntity>>(categories);
    }

    public async Task<CategoryEntity> GetByName(string name)
    {
        var category = await _dbContext.Categories.FirstOrDefaultAsync(c => c.Name == name);

        if (category == null)
        {
            throw new NoDataFoundException($"Category {name} not found");
        }

        return _mapper.Map<CategoryEntity>(category);

    }
}

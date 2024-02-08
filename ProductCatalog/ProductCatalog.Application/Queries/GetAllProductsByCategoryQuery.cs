using AutoMapper;
using MediatR;
using ProductCatalog.Application.Resources;
using ProductCatalog.Domain.Repository;

namespace ProductCatalog.Application.Queries;

public record GetAllProductsByCategoryQuery(string CategoryName) : IRequest<IEnumerable<Product>>;

public class GetAllProductsByCategoryQueryHandler : IRequestHandler<GetAllProductsByCategoryQuery, IEnumerable<Product>>
{
    private readonly IProductCatalogRepository _productCatalogRepository;
    private IMapper _mapper;


    public GetAllProductsByCategoryQueryHandler(IProductCatalogRepository productCatalogRepository, IMapper mapper)
    {
        _productCatalogRepository = productCatalogRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<Product>> Handle(GetAllProductsByCategoryQuery request, CancellationToken cancellationToken)
    {
        var products = await _productCatalogRepository.GetByCategoryAsync(request.CategoryName);
        return _mapper.Map<IEnumerable<Product>>(products);
    }
}
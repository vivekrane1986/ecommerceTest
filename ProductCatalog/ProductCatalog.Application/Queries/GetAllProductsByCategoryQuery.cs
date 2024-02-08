using AutoMapper;
using MediatR;
using ProductCatalog.Domain.Entities;
using ProductCatalog.Domain.Repository;

namespace ProductCatalog.Application.Queries;

public record GetAllProductsByCategoryQuery(string CategoryName) : IRequest<IEnumerable<ProductEntity>>;

public class GetAllProductsByCategoryQueryHandler : IRequestHandler<GetAllProductsByCategoryQuery, IEnumerable<ProductEntity>>
{
    private readonly IProductCatalogRepository _productCatalogRepository;
    private IMapper _mapper;


    public GetAllProductsByCategoryQueryHandler(IProductCatalogRepository productCatalogRepository, IMapper mapper)
    {
        _productCatalogRepository = productCatalogRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ProductEntity>> Handle(GetAllProductsByCategoryQuery request, CancellationToken cancellationToken)
    {
        return await _productCatalogRepository.GetByCategoryAsync(request.CategoryName);        
    }
}
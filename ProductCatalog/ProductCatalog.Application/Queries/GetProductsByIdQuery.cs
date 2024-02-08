using AutoMapper;
using MediatR;
using ProductCatalog.Application.Resources;
using ProductCatalog.Domain.Repository;

namespace ProductCatalog.Application.Queries;

public record GetProductsByIdQuery(Guid id) : IRequest<Product>;

public class GetProductsByIdQueryHandler : IRequestHandler<GetProductsByIdQuery, Product>
{
    private IProductCatalogRepository _productCatelogRepository;
    private IMapper _mapper;

    public GetProductsByIdQueryHandler(IProductCatalogRepository productCatelogRepository, IMapper mapper)
    {
        _productCatelogRepository = productCatelogRepository;
        _mapper = mapper;
    }

    public async Task<Product> Handle(GetProductsByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _productCatelogRepository.GetByIdAsync(request.id);

        return _mapper.Map<Product>(product);
    }
}
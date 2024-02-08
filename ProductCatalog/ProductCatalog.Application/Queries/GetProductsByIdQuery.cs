using AutoMapper;
using MediatR;
using ProductCatalog.Domain.Entities;
using ProductCatalog.Domain.Repository;

namespace ProductCatalog.Application.Queries;

public record GetProductsByIdQuery(Guid id) : IRequest<ProductEntity>;

public class GetProductsByIdQueryHandler : IRequestHandler<GetProductsByIdQuery, ProductEntity>
{
    private IProductCatalogRepository _productCatelogRepository;
    private IMapper _mapper;

    public GetProductsByIdQueryHandler(IProductCatalogRepository productCatelogRepository, IMapper mapper)
    {
        _productCatelogRepository = productCatelogRepository;
        _mapper = mapper;
    }

    public async Task<ProductEntity> Handle(GetProductsByIdQuery request, CancellationToken cancellationToken)
    {
       return await _productCatelogRepository.GetByIdAsync(request.id);        
    }
}
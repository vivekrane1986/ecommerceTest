using FluentValidation;
using MediatR;
using ProductCatalog.Domain.Entities;
using ProductCatalog.Domain.Repository;

namespace ProductCatalog.Application.Commands;

public record AddProductCommand(string Name, string Description, string CategoryName, string Code) : IRequest;

public class AddProductCommandHandler : IRequestHandler<AddProductCommand>
{
    private readonly IProductCatalogRepository _productCatalogRepository;
    private readonly IValidator<AddProductCommand> _validator;

    public AddProductCommandHandler(IProductCatalogRepository productCatalogRepository, IValidator<AddProductCommand> validator)
    {
        _productCatalogRepository = productCatalogRepository;
        _validator = validator;
    }

    public async Task Handle(AddProductCommand request, CancellationToken cancellationToken)
    {
        var product = new ProductEntity() { Id = Guid.NewGuid(), Name = request.Name, Description = request.Description, CategoryName = request.CategoryName, Code=request.Code };
        
        await _validator.ValidateAndThrowAsync(request);

        await _productCatalogRepository.AddAsync(product);
    }
}

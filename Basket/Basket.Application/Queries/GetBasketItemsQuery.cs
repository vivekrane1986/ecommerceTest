using Basket.Domain;
using Basket.Domain.Entities;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace Basket.Application.Queries;

public record GetBasketItemsQuery(string CustomerId) : IRequest<IEnumerable<BasketEntity>>;

[ExcludeFromCodeCoverage(Justification = "Sample code so not covering all Classes")]
public class GetBasketItemsQueryHandler : IRequestHandler<GetBasketItemsQuery, IEnumerable<BasketEntity>>
{
    private readonly IBasketRepository _basketRepository;  

    public GetBasketItemsQueryHandler(IBasketRepository basketRepository)
    {
        _basketRepository = basketRepository;   
    }

    public async Task<IEnumerable<BasketEntity>> Handle(GetBasketItemsQuery request, CancellationToken cancellationToken)
    {
        return await _basketRepository.GetAllBasketItemsAsync(request.CustomerId);
    }
}
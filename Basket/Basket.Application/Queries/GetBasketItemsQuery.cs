using AutoMapper;
using Basket.Domain;
using Basket.Domain.Entities;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace Basket.Application.Queries;

public record GetBasketItemsQuery(Guid OrderId) : IRequest<IEnumerable<BasketEntity>>;

[ExcludeFromCodeCoverage(Justification = "Sample code so not covering all Classes")]
public class GetBasketItemsQueryHandler : IRequestHandler<GetBasketItemsQuery, IEnumerable<BasketEntity>>
{
    private readonly IBasketRepository _basketRepository;
    private IMapper _mapper;


    public GetBasketItemsQueryHandler(IBasketRepository basketRepository, IMapper mapper)
    {
        _basketRepository = basketRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<BasketEntity>> Handle(GetBasketItemsQuery request, CancellationToken cancellationToken)
    {
        return await _basketRepository.GetAllBasketItemsAsync(request.OrderId);
    }
}
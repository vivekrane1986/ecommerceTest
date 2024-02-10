using AutoMapper;
using Basket.Domain;
using Basket.Domain.Entities;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace Basket.Application.Command;

public record CheckoutBasketCommand(List<BasketEntity> BasketEntity) : IRequest<bool>;


[ExcludeFromCodeCoverage(Justification = "Sample code so not covering all Classes")]
public class CheckoutBasketCommandHandler : IRequestHandler<CheckoutBasketCommand, bool>
{
    private IBasketRepository _basketRepository;    

    public CheckoutBasketCommandHandler(IBasketRepository basketRepository, IMapper mapper)
    {
        _basketRepository = basketRepository;
    }

    public async Task<bool> Handle(CheckoutBasketCommand request, CancellationToken cancellationToken)
    {
        await _basketRepository.SaveBasketDetailsAsync(request.BasketEntity);

        return true;
    }
}
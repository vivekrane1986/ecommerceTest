using Basket.Domain;
using Basket.Domain.Entities;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace Basket.Application.Command;

public record RemoveItemInBasketCommand(BasketEntity BasketEntity) : IRequest<bool>;

[ExcludeFromCodeCoverage(Justification = "Sample code so not covering all Classes")]
public class RemoveItemInBasketQueryHandler : IRequestHandler<RemoveItemInBasketCommand, bool>
{
    private IBasketRepository _basketRepository;

    public RemoveItemInBasketQueryHandler(IBasketRepository basketRepository)
    {
        _basketRepository = basketRepository;
    }

    public async Task<bool> Handle(RemoveItemInBasketCommand request, CancellationToken cancellationToken)
    {
        return await _basketRepository.RemoveItemAsync(request.BasketEntity);
    }
}

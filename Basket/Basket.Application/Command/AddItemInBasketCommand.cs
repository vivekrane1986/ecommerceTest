using Basket.Domain;
using Basket.Domain.Entities;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace Basket.Application.Command;

public record AddItemInBasketCommand(BasketEntity BasketEntity) : IRequest<string>;


[ExcludeFromCodeCoverage(Justification = "Sample code so not covering all Classes")]
public class AddItemInBasketCommandHandler : IRequestHandler<AddItemInBasketCommand, string>
{
    private IBasketRepository _basketRepository;    

    public AddItemInBasketCommandHandler(IBasketRepository basketRepository)
    {
        _basketRepository = basketRepository;
    }

    public async Task<string> Handle(AddItemInBasketCommand request, CancellationToken cancellationToken)
    {
        return await _basketRepository.AddItemAsync(request.BasketEntity);
    }
}
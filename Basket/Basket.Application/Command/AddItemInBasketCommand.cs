using AutoMapper;
using Basket.Domain;
using Basket.Domain.Entities;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace Basket.Application.Command;

public record AddItemInBasketCommand(BasketEntity BasketEntity) : IRequest<bool>;


[ExcludeFromCodeCoverage(Justification = "Sample code so not covering all Classes")]
public class AddItemInBasketCommandHandler : IRequestHandler<AddItemInBasketCommand, bool>
{
    private IBasketRepository _basketRepository;    

    public AddItemInBasketCommandHandler(IBasketRepository basketRepository, IMapper mapper)
    {
        _basketRepository = basketRepository;
    }

    public async Task<bool> Handle(AddItemInBasketCommand request, CancellationToken cancellationToken)
    {
        return await _basketRepository.AddItemAsync(request.BasketEntity);
    }
}
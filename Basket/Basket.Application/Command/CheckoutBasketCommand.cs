using Basket.Domain;
using Basket.Domain.Entities;
using Basket.Domain.Services;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace Basket.Application.Command;

public record CheckoutBasketCommand(string? CustomerId,List<BasketEntity> BasketEntity) : IRequest<bool>;


[ExcludeFromCodeCoverage(Justification = "Sample code - not covering all Classes")]
public class CheckoutBasketCommandHandler : IRequestHandler<CheckoutBasketCommand, bool>
{
    private IBasketRepository _basketRepository;
    private IMembershipService _membershipService;
    private IMessagePublisher _messagePublisher;

    public CheckoutBasketCommandHandler(IBasketRepository basketRepository, IMembershipService membershipService, IMessagePublisher messagePublisher)
    {
        _basketRepository = basketRepository;
        _membershipService = membershipService;
        _messagePublisher = messagePublisher;
    }

    public async Task<bool> Handle(CheckoutBasketCommand request, CancellationToken cancellationToken)
    {
        await _basketRepository.SaveBasketDetailsAsync(request.BasketEntity);

        var discountPerc = await _membershipService.GetMembershipDiscountAsync(request.CustomerId);

        await _messagePublisher.PublishAsync(request.BasketEntity);

        return true;
    }
}
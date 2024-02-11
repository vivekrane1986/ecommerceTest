using Basket.Application.Command;
using Basket.Application.Queries;
using Basket.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;


namespace Basket.API.Controllers
{
    [ExcludeFromCodeCoverage(Justification = "Sample code - not covering all Classes")]
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IMediator _sender;

        public BasketController(IMediator sender)
        {
            _sender = sender;
        }

        [HttpGet("{customerId}")]
        public async Task<IEnumerable<BasketEntity>> GetBasketItems(string customerId)
        {
            return await _sender.Send(new GetBasketItemsQuery(customerId));
        }

        [HttpPost("AddItemInBasket")]
        public async Task<string> AddItem(BasketEntity basket)
        {
            return await _sender.Send(new AddItemInBasketCommand(basket));
        }

        [HttpPost("checkout/{customerId}")]
        public async Task Checkout(string customerId, [FromBody] List<BasketEntity> basket)
        {
             await _sender.Send(new CheckoutBasketCommand(customerId, basket));
        }

    }
}

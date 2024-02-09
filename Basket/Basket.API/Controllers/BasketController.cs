using Basket.Application.Command;
using Basket.Application.Queries;
using Basket.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;


namespace Basket.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IMediator _sender;

        public BasketController(IMediator sender)
        {
            _sender = sender;
        }

        [HttpGet("{basketId}")]
        public async Task<IEnumerable<BasketEntity>> GetBasketItems(Guid basketId)
        {
            return await _sender.Send(new GetBasketItemsQuery(basketId));
        }


        [HttpPost("checkout")]
        public async Task Checkout([FromBody] List<BasketEntity> basket)
        {
             await _sender.Send(new CheckoutBasketCommand(basket));
        }

    }
}

using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Application.Commands;
using ProductCatalog.Application.Queries;
using ProductCatalog.Domain.Entities;


namespace ProductCatalog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductCatalogController : ControllerBase
    {
        private readonly IMediator _sender;      

        public ProductCatalogController(IMediator sender)
        {
            _sender = sender;
        }

        [HttpGet("category/{categoryName}")]
        public async Task<IEnumerable<ProductEntity>> GetByCategory(string categoryName)
        {
            return await _sender.Send(new GetAllProductsByCategoryQuery(categoryName));
        }


        [HttpGet("{id}")]
        public async Task<ProductEntity> Get(Guid id)
        {
            return await _sender.Send(new GetProductsByIdQuery(id));
        }

        [HttpPost]
        public async Task Post([FromBody] ProductEntity product)
        {
            await _sender.Send(new AddProductCommand(product.Name,product.Description,product.CategoryName,product.Code));
        }
        
    }
}

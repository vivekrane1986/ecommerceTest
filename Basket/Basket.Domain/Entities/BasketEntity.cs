namespace Basket.Domain.Entities;

public class BasketEntity
{
    public Guid OrderId { get; set; }

    public string CustomerId { get; set; }

    public string ProductCode { get; set; }

    public int Quantity { get; set; }

    public decimal Price { get; set; }
}

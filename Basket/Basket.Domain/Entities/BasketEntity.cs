using System.Diagnostics.CodeAnalysis;

namespace Basket.Domain.Entities;

[ExcludeFromCodeCoverage(Justification = "Sample code - not covering all Classes")]
public class BasketEntity
{
    public string CustomerId { get; set; }

    public string ProductCode { get; set; }

    public int Quantity { get; set; }

    public decimal Price { get; set; }
}

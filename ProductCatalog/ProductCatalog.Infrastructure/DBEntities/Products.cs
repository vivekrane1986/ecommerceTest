
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductCatalog.Infrastructure.DBEntities;

public class Product
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public Guid CategoryId { get; set; }

    [ForeignKey(nameof(CategoryId))]
    public Category Category { get; set; }
}

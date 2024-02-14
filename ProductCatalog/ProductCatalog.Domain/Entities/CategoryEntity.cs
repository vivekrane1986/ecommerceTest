namespace ProductCatalog.Domain.Entities;

public class CategoryEntity
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string? Description { get; set; } 
}

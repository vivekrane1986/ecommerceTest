using System.ComponentModel.DataAnnotations.Schema;

namespace ProductCatalog.Infrastructure.DBEntities;

[Table(name: "Categories")]
public class Category
{
    public Guid Id { get; set; }

    public  string Name { get; set; }

    public string Description { get; set; }

}
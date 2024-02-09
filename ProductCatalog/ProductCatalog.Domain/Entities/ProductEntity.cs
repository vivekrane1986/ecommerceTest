﻿namespace ProductCatalog.Domain.Entities;

public class ProductEntity
{
    public Guid Id { get; set; }

    public string Code { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public decimal Price { get; set; }

    public string CategoryName { get; set; }
}

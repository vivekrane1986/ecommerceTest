using AutoMapper;
using ProductCatalog.Domain.Entities;
using ProductCatalog.Infrastructure.DBEntities;

namespace ProductCatalog.Infrastructure;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ProductEntity, Product>().ReverseMap();

        CreateMap<CategoryEntity, Category>().ReverseMap();     
    }
}

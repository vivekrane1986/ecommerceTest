using AutoMapper;
using ProductCatalog.Application.Resources;
using ProductCatalog.Domain.Entities;

namespace ProductCatalog.Application;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ProductEntity, Product>().ReverseMap();
    }
}

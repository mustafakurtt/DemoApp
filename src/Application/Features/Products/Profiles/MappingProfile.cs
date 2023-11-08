using Application.Features.Products.Commands.Create;
using Application.Features.Products.Commands.Delete;
using Application.Features.Products.Commands.Update;
using Application.Features.Products.Queries.GetById;
using Application.Features.Products.Queries.GetList;
using AutoMapper;
using Domain.Entities;

namespace Application.Features.Products.Profiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Product, CreateProductCommand>().ReverseMap();
        CreateMap<Product, CreatedProductCommandResponse>().ReverseMap();

        CreateMap<Product, UpdateProductCommand>().ReverseMap();
        CreateMap<Product, UpdatedProductCommandResponse>().ReverseMap();

        CreateMap<Product, DeleteProductCommand>().ReverseMap();
        CreateMap<Product, DeletedProductCommandResponse>().ReverseMap();

        CreateMap<Product, GetByIdProductQueryResponse>()
            .ForMember(destinationMember: dest => dest.CategoryName,
                memberOptions: opt => opt.MapFrom(ct => ct.Category.Name))
            .ForMember(destinationMember: dest => dest.SupplierName,
                memberOptions: opt => opt.MapFrom(ct => ct.Supplier.Name))
            .ReverseMap();

        CreateMap<Product, GetListProductQueryResponse>()
            .ForMember(destinationMember: dest => dest.CategoryName,
                memberOptions: opt => opt.MapFrom(ct => ct.Category.Name))
            .ForMember(destinationMember: dest => dest.SupplierName,
                memberOptions: opt => opt.MapFrom(ct => ct.Supplier.Name))
            .ReverseMap();
    }
}
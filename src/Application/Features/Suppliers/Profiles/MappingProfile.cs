using Application.Features.Suppliers.Commands.Create;
using Application.Features.Suppliers.Commands.Delete;
using Application.Features.Suppliers.Commands.Update;
using Application.Features.Suppliers.Queries.GetById;
using Application.Features.Suppliers.Queries.GetList;
using AutoMapper;
using Domain.Entities;

namespace Application.Features.Suppliers.Profiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Supplier, CreateSupplierCommand>().ReverseMap();
        CreateMap<Supplier, CreatedSupplierCommandResponse>().ReverseMap();

        CreateMap<Supplier, UpdateSupplierCommand>().ReverseMap();
        CreateMap<Supplier, UpdatedSupplierCommandResponse>().ReverseMap();

        CreateMap<Supplier, DeleteSupplierCommand>().ReverseMap();
        CreateMap<Supplier, DeletedSupplierCommandResponse>().ReverseMap();

        CreateMap<Supplier, GetListSupplierQueryResponse>().ReverseMap();
        CreateMap<Supplier, GetByIdSupplierQueryResponse>().ReverseMap();
    }
}
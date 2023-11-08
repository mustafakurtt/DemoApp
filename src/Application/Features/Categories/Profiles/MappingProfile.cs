using Application.Features.Categories.Commands.Create;
using Application.Features.Categories.Commands.Delete;
using Application.Features.Categories.Commands.Update;
using Application.Features.Categories.Queries.GetById;
using Application.Features.Categories.Queries.GetList;
using AutoMapper;
using Domain.Entities;

namespace Application.Features.Categories.Profiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Category, CreateCategoryCommand>().ReverseMap();
        CreateMap<Category, CreatedCategoryCommandResponse>().ReverseMap();

        CreateMap<Category, UpdateCategoryCommand>().ReverseMap();
        CreateMap<Category, UpdatedCategoryCommandResponse>().ReverseMap();

        CreateMap<Category, DeletedCategoryCommandResponse>().ReverseMap();
        CreateMap<Category, DeleteCategoryCommand>().ReverseMap();

        CreateMap<GetListCategoryQueryResponse, Category>().ReverseMap();
        CreateMap<GetByIdCategoryQueryResponse, Category>().ReverseMap();
    }
}
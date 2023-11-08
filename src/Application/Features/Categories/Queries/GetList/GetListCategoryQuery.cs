using Application.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.Categories.Queries.GetList;

public class GetListCategoryQuery : IRequest<List<GetListCategoryQueryResponse>>
{
    public class GetListCategoryQueryHandler : IRequestHandler<GetListCategoryQuery, List<GetListCategoryQueryResponse>>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public GetListCategoryQueryHandler(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<List<GetListCategoryQueryResponse>> Handle(GetListCategoryQuery request, CancellationToken cancellationToken)
        {
            List<Category> categories = await _categoryRepository.GetListAsync();
            List<GetListCategoryQueryResponse> responses = _mapper.Map<List<GetListCategoryQueryResponse>>(categories);
            return responses;
        }
    }
}
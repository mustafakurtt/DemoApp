using Application.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Products.Queries.GetList;

public class GetListProductQuery : IRequest<List<GetListProductQueryResponse>>
{
    public class GetListProductQueryHandler : IRequestHandler<GetListProductQuery, List<GetListProductQueryResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;

        public GetListProductQueryHandler(IMapper mapper, IProductRepository productRepository)
        {
            _mapper = mapper;
            _productRepository = productRepository;
        }

        public async Task<List<GetListProductQueryResponse>> Handle(GetListProductQuery request, CancellationToken cancellationToken)
        {
            List<Product> products = await _productRepository.GetListAsync(
                include: p => p.Include(m => m.Category).Include(m => m.Supplier));
            List<GetListProductQueryResponse> response = _mapper.Map<List<GetListProductQueryResponse>>(products);
            return response;
        }
    }
}
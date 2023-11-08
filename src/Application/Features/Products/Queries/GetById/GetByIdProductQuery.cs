using Application.Features.Products.Rules;
using Application.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Products.Queries.GetById;

public class GetByIdProductQuery : IRequest<GetByIdProductQueryResponse>
{
    public int Id { get; set; }

    public class GetByIdProductQueryHandler : IRequestHandler<GetByIdProductQuery, GetByIdProductQueryResponse>
    {
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;
        private readonly ProductBusinessRules _businessRules;

        public GetByIdProductQueryHandler(IMapper mapper, IProductRepository productRepository, ProductBusinessRules businessRules)
        {
            _mapper = mapper;
            _productRepository = productRepository;
            _businessRules = businessRules;
        }

        public async Task<GetByIdProductQueryResponse> Handle(GetByIdProductQuery request, CancellationToken cancellationToken)
        {
            Product? product = await _productRepository.GetAsync(
                p => p.Id == request.Id,
                include: p => p.Include(m => m.Category).Include(m => m.Supplier),
                cancellationToken: cancellationToken);
            await _businessRules.CategoryShouldExistsWhenSelected(product);
            GetByIdProductQueryResponse response = _mapper.Map<GetByIdProductQueryResponse>(product);
            return response;
        }
    }
}
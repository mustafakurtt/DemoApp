using Application.Features.Products.Rules;
using Application.Repositories;
using AutoMapper;
using Core.Application.Pipelines.Transaction;
using Core.Persistence;
using Domain.Entities;
using MediatR;

namespace Application.Features.Products.Commands.Create;

public class CreateProductCommand : IRequest<CreatedProductCommandResponse>, ITransactionalRequest
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int CategoryId { get; set; }
    public int SupplierId { get; set; }

    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, CreatedProductCommandResponse>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly IProductBusinessRules _businessRules;
        private readonly IUnitOfWork<IProductRepository> _unitOfWork;

        public CreateProductCommandHandler(IProductRepository productRepository, IUnitOfWork<IProductRepository> unitOfWork, IMapper mapper, IProductBusinessRules businessRules)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _businessRules = businessRules;
            _unitOfWork = unitOfWork;
        }

        public async Task<CreatedProductCommandResponse> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            Product product = _mapper.Map<Product>(request);
            await _businessRules.CategoryShouldExistsWhenSelected(product);
            await _businessRules.SupplierShouldExistsWhenSelected(product);
            await _businessRules.ProductNameCanNotBeDuplicatedWhenInserted(product);
            await _productRepository.AddAsync(product);
            await _unitOfWork.SaveChangesAsync();

            CreatedProductCommandResponse response = _mapper.Map<CreatedProductCommandResponse>(product);


            return response;
        }
    }
}
using Application.Features.Products.Rules;
using Application.Repositories;
using AutoMapper;
using Core.Application.Pipelines.Transaction;
using Core.Persistence;
using Domain.Entities;
using MediatR;

namespace Application.Features.Products.Commands.Update;

public class UpdateProductCommand : IRequest<UpdatedProductCommandResponse>, ITransactionalRequest
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int CategoryId { get; set; }
    public int SupplierId { get; set; }

    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, UpdatedProductCommandResponse>
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork<IProductRepository> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IProductBusinessRules _businessRules;

        public UpdateProductCommandHandler(IProductRepository productRepository, IUnitOfWork<IProductRepository> unitOfWork, IMapper mapper, IProductBusinessRules businessRules)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _businessRules = businessRules;
            _unitOfWork = unitOfWork;
        }

        public async Task<UpdatedProductCommandResponse> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            Product? product = await _productRepository.GetAsync(p => p.Id == request.Id, cancellationToken: cancellationToken);

            await _businessRules.ProductShouldExistsWhenSelected(product);
            await _businessRules.ProductNameCanNotBeDuplicatedWhenUpdated(product);
            await _businessRules.SupplierShouldExistsWhenSelected(product);
            await _businessRules.CategoryShouldExistsWhenSelected(product);

            product = _mapper.Map(request, product);

            await _productRepository.UpdateAsync(product);
            await _unitOfWork.SaveChangesAsync();

            UpdatedProductCommandResponse response = _mapper.Map<UpdatedProductCommandResponse>(product);

            return response;
        }
    }

}
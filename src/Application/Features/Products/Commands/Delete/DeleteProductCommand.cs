using Application.Features.Products.Rules;
using Application.Repositories;
using AutoMapper;
using Core.Application.Pipelines.Transaction;
using Core.Persistence;
using Domain.Entities;
using MediatR;

namespace Application.Features.Products.Commands.Delete;

public class DeleteProductCommand : IRequest<DeletedProductCommandResponse>, ITransactionalRequest
{
    public int Id { get; set; }
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, DeletedProductCommandResponse>
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork<IProductRepository> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IProductBusinessRules _productBusinessRules;

        public DeleteProductCommandHandler(IProductRepository productRepository, IUnitOfWork<IProductRepository> unitOfWork, IMapper mapper, IProductBusinessRules productBusinessRules)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _productBusinessRules = productBusinessRules;
            _unitOfWork = unitOfWork;
        }

        public async Task<DeletedProductCommandResponse> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            Product? product = await _productRepository.GetAsync(p => p.Id == request.Id);

            await _productBusinessRules.ProductShouldExistsWhenSelected(product);

            await _productRepository.DeleteAsync(product);
            await _unitOfWork.SaveChangesAsync();

            DeletedProductCommandResponse response = _mapper.Map<DeletedProductCommandResponse>(product);


            return response;
        }
    }
}
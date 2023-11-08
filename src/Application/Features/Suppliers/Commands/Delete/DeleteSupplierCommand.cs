using Application.Features.Suppliers.Rules;
using Application.Repositories;
using AutoMapper;
using Core.Application.Pipelines.Transaction;
using Core.Persistence;
using Domain.Entities;
using MediatR;

namespace Application.Features.Suppliers.Commands.Delete;

public class DeleteSupplierCommand : IRequest<DeletedSupplierCommandResponse>, ITransactionalRequest
{
    public int Id { get; set; }

    public class DeleteSupplierCommandHandler : IRequestHandler<DeleteSupplierCommand, DeletedSupplierCommandResponse>
    {
        private readonly IMapper _mapper;
        private readonly ISupplierRepository _supplierRepository;
        private readonly ISupplierBusinessRules _supplierBusinessRules;
        private readonly IUnitOfWork<ISupplierRepository> _unitOfWork;

        public DeleteSupplierCommandHandler(ISupplierRepository supplierRepository, IUnitOfWork<ISupplierRepository> unitOfWork, IMapper mapper, ISupplierBusinessRules supplierBusinessRules)
        {
            _mapper = mapper;
            _supplierRepository = supplierRepository;
            _supplierBusinessRules = supplierBusinessRules;
            _unitOfWork = unitOfWork;
        }

        public async Task<DeletedSupplierCommandResponse> Handle(DeleteSupplierCommand request, CancellationToken cancellationToken)
        {
            Supplier? supplier =
                await _supplierRepository.GetAsync(p => p.Id == request.Id, cancellationToken: cancellationToken);
            await _supplierBusinessRules.SupplierShouldExistsWhenSelected(supplier);

            await _supplierRepository.DeleteAsync(supplier);
            await _unitOfWork.SaveChangesAsync();

            DeletedSupplierCommandResponse response = _mapper.Map<DeletedSupplierCommandResponse>(supplier);


            return response;

        }
    }
}
using Application.Features.Suppliers.Rules;
using Application.Repositories;
using AutoMapper;
using Core.Application.Pipelines.Transaction;
using Core.Persistence;
using Domain.Entities;
using MediatR;

namespace Application.Features.Suppliers.Commands.Create;

public class CreateSupplierCommand : IRequest<CreatedSupplierCommandResponse>, ITransactionalRequest
{
    public string Name { get; set; }

    public class CreateSupplierCommandHandler : IRequestHandler<CreateSupplierCommand, CreatedSupplierCommandResponse>
    {
        private readonly ISupplierRepository _supplierRepository;
        private readonly IUnitOfWork<ISupplierRepository> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ISupplierBusinessRules _supplierBusinessRules;

        public CreateSupplierCommandHandler(ISupplierRepository supplierRepository, IUnitOfWork<ISupplierRepository> unitOfWork, IMapper mapper, ISupplierBusinessRules supplierBusinessRules)
        {
            _supplierRepository = supplierRepository;
            _mapper = mapper;
            _supplierBusinessRules = supplierBusinessRules;
            _unitOfWork = unitOfWork;
        }

        public async Task<CreatedSupplierCommandResponse> Handle(CreateSupplierCommand request, CancellationToken cancellationToken)
        {
            Supplier supplier = _mapper.Map<Supplier>(request);

            await _supplierBusinessRules.SupplierNameCanNotBeDuplicatedWhenInserted(supplier.Name);
            await _supplierRepository.AddAsync(supplier);
            await _unitOfWork.SaveChangesAsync();

            CreatedSupplierCommandResponse response = _mapper.Map<CreatedSupplierCommandResponse>(supplier);
            return response;
        }
    }
}
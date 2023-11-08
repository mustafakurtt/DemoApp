using Application.Features.Suppliers.Rules;
using Application.Repositories;
using AutoMapper;
using Core.Application.Pipelines.Transaction;
using Core.Persistence;
using Domain.Entities;
using MediatR;

namespace Application.Features.Suppliers.Commands.Update;

public class UpdateSupplierCommand : IRequest<UpdatedSupplierCommandResponse>, ITransactionalRequest
{
    public int Id { get; set; }
    public string Name { get; set; }

    public class UpdateSupplierCommandHandler : IRequestHandler<UpdateSupplierCommand, UpdatedSupplierCommandResponse>
    {
        private readonly ISupplierRepository _supplierRepository;
        private readonly IMapper _mapper;
        private readonly ISupplierBusinessRules _supplierBusinessRules;
        private readonly IUnitOfWork<ISupplierRepository> _unitOfWork;

        public UpdateSupplierCommandHandler(ISupplierRepository supplierRepository, IUnitOfWork<ISupplierRepository> unitOfWork, IMapper mapper, ISupplierBusinessRules supplierBusinessRules)
        {
            _supplierRepository = supplierRepository;
            _mapper = mapper;
            _supplierBusinessRules = supplierBusinessRules;
            _unitOfWork = unitOfWork;
        }

        public async Task<UpdatedSupplierCommandResponse> Handle(UpdateSupplierCommand request, CancellationToken cancellationToken)
        {
            Supplier? supplier = await _supplierRepository.GetAsync(s => s.Id == request.Id);
            await _supplierBusinessRules.SupplierShouldExistsWhenSelected(supplier);
            supplier = _mapper.Map(request, supplier);
            await _supplierRepository.UpdateAsync(supplier);
            await _unitOfWork.SaveChangesAsync();
            UpdatedSupplierCommandResponse response = _mapper.Map<UpdatedSupplierCommandResponse>(supplier);


            return response;
        }
    }
}
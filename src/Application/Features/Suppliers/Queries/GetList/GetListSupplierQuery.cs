using Application.Features.Suppliers.Rules;
using Application.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.Suppliers.Queries.GetList;

public class GetListSupplierQuery : IRequest<List<GetListSupplierQueryResponse>>
{
    public class GetListSupplierQueryHandler : IRequestHandler<GetListSupplierQuery, List<GetListSupplierQueryResponse>>
    {
        private readonly ISupplierRepository _supplierRepository;
        private readonly IMapper _mapper;
        private readonly SupplierBusinessRules _supplierBusinessRules;

        public GetListSupplierQueryHandler(ISupplierRepository supplierRepository, IMapper mapper, SupplierBusinessRules supplierBusinessRules)
        {
            _supplierRepository = supplierRepository;
            _mapper = mapper;
            _supplierBusinessRules = supplierBusinessRules;
        }

        public async Task<List<GetListSupplierQueryResponse>> Handle(GetListSupplierQuery request, CancellationToken cancellationToken)
        {
            List<Supplier> suppliers = await _supplierRepository.GetListAsync();
            List<GetListSupplierQueryResponse> responses = _mapper.Map<List<GetListSupplierQueryResponse>>(suppliers);
            return responses;
        }
    }
}
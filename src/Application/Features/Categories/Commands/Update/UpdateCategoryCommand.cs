using Application.Features.Categories.Rules;
using Application.Repositories;
using AutoMapper;
using Core.Application.Pipelines.Transaction;
using Core.Persistence;
using Domain.Entities;
using MediatR;

namespace Application.Features.Categories.Commands.Update;
public class UpdateCategoryCommand : IRequest<UpdatedCategoryCommandResponse>, ITransactionalRequest
{
    public int Id { get; set; }
    public string Name { get; set; }

    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, UpdatedCategoryCommandResponse>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly ICategoryBusinessRules _categoryBusinessRules;
        private readonly IUnitOfWork<ICategoryRepository> _unitOfWork;
        public UpdateCategoryCommandHandler(ICategoryRepository categoryRepository, IUnitOfWork<ICategoryRepository> unitOfWork, IMapper mapper, ICategoryBusinessRules businessRules)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _categoryBusinessRules = businessRules;
            _unitOfWork = unitOfWork;
        }
        public async Task<UpdatedCategoryCommandResponse> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            Category? category = await _categoryRepository.GetAsync(predicate: c => c.Id == request.Id, cancellationToken: cancellationToken);
            await _categoryBusinessRules.CategoryShouldExistsWhenSelected(category);
            category = _mapper.Map(request, category);

            await _categoryRepository.UpdateAsync(category);
            await _unitOfWork.SaveChangesAsync();

            UpdatedCategoryCommandResponse response = _mapper.Map<UpdatedCategoryCommandResponse>(category);
            return response;
        }
    }
}
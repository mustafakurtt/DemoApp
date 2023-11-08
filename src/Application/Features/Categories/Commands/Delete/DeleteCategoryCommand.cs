using Application.Features.Categories.Rules;
using Application.Repositories;
using AutoMapper;
using Core.Application.Pipelines.Transaction;
using Core.Persistence;
using Domain.Entities;
using MediatR;

namespace Application.Features.Categories.Commands.Delete;

public class DeleteCategoryCommand : IRequest<DeletedCategoryCommandResponse>, ITransactionalRequest
{
    public int Id { get; set; }

    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, DeletedCategoryCommandResponse>
    {
        private readonly IMapper _mapper;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork<ICategoryRepository> _unitOfWork;
        private readonly ICategoryBusinessRules _categoryBusinessRules;

        public DeleteCategoryCommandHandler(ICategoryRepository categoryRepository, IUnitOfWork<ICategoryRepository> unitOfWork, IMapper mapper, ICategoryBusinessRules categoryBusinessRules)
        {
            _mapper = mapper;
            _categoryRepository = categoryRepository;
            _categoryBusinessRules = categoryBusinessRules;
            _unitOfWork = unitOfWork;
        }

        public async Task<DeletedCategoryCommandResponse> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            Category? category = await _categoryRepository.GetAsync(predicate: u => u.Id == request.Id, cancellationToken: cancellationToken);
            await _categoryBusinessRules.CategoryShouldExistsWhenSelected(category);

            await _categoryRepository.DeleteAsync(category!);
            await _unitOfWork.SaveChangesAsync();

            DeletedCategoryCommandResponse response = _mapper.Map<DeletedCategoryCommandResponse>(category);


            return response;
        }
    }
}
using Application.Features.Categories.Rules;
using Application.Repositories;
using AutoMapper;
using Core.Application.Pipelines.Transaction;
using Core.Persistence;
using Domain.Entities;
using MediatR;

namespace Application.Features.Categories.Commands.Create;

public class CreateCategoryCommand : IRequest<CreatedCategoryCommandResponse>, ITransactionalRequest
{
    public string Name { get; set; }

    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, CreatedCategoryCommandResponse>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork<ICategoryRepository> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICategoryBusinessRules _categoryBusinessRules;


        public CreateCategoryCommandHandler(ICategoryRepository categoryRepository, IUnitOfWork<ICategoryRepository> unitOfWork, IMapper mapper, ICategoryBusinessRules businessRules)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _categoryBusinessRules = businessRules;
            _unitOfWork = unitOfWork;
        }

        public async Task<CreatedCategoryCommandResponse> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            Category category = _mapper.Map<Category>(request);
            await _categoryBusinessRules.CategoryNameCanNotBeDuplicatedWhenInserted(category.Name);
            await _categoryRepository.AddAsync(category);
            await _unitOfWork.SaveChangesAsync();

            CreatedCategoryCommandResponse response = _mapper.Map<CreatedCategoryCommandResponse>(category);


            return response;
        }
    }
}
using AutoMapper;
using FluentValidation;
using ProjektZTP.Models;
using ProjektZTP.Repository.Interfaces;
using static ProjektZTP.Mediator.Abstract;

namespace ProjektZTP.Features.OrderFeatures.Commands;

public class AddOrder
{
    private readonly IProductsRepository _repositoryProducts;
    private readonly IUserRepository _repositoryUsers;

    public AddOrder(IProductsRepository repositoryProducts, IUserRepository repositoryUsers = null)
    {
        _repositoryProducts = repositoryProducts;
        _repositoryUsers = repositoryUsers;
    }

    public class AddOrderValidator : AbstractValidator<AddOrderCommand>
    {
        public AddOrderValidator()
        {
            //there will be another validation for UserId or ProductId which doesnt exist in database.
            RuleFor(x => x.Customer).NotEmpty().NotNull();
            RuleFor(x => x.Address).NotEmpty().NotNull();
            RuleFor(x => x.UserId).NotEmpty().NotNull();
            RuleFor(x => x.ProductIds).NotEmpty().NotNull();
        }
    }

    public record AddOrderCommand(
    string Customer,
    string Address,
    Guid UserId,
    ICollection<Guid> ProductIds) : IRequest<AddOrderCommandResult>;

    public class AddOrderCommandHandler : IHandler<AddOrderCommand, AddOrderCommandResult>
    {
        private readonly IOrdersRepository _repository;
        private readonly IMapper _mapper;

        public AddOrderCommandHandler(IOrdersRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<AddOrderCommandResult> HandleAsync(AddOrderCommand request)
        {
            var validator = new AddOrderValidator();

            var result = validator.Validate(request);

            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }

            var entry = _mapper.Map<Order>(request);
            await _repository.Add(entry);
            //to change
            return new AddOrderCommandResult(entry.Id);
        }
    }

    public record AddOrderCommandResult(Guid id);
}



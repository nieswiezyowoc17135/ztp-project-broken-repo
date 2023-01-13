using AutoMapper;
using FluentValidation;
using MediatR;
using ProjektZTP.Models;
using ProjektZTP.Repository.Interfaces;


namespace ProjektZTP.Features.OrderFeatures.Commands;

public class AddOrder
{
    public record Command(
        string Customer,
        string Address,
        Guid UserId,
        ICollection<Guid> ProductIds) : IRequest<Validator.Result>;

    public class Validator : AbstractValidator<Command>
    {
        private readonly IProductsRepository _repositoryProducts;
        private readonly IUserRepository _repositoryUsers;

        public Validator(IProductsRepository repositoryProducts, IUserRepository repositoryUsers)
        {
            _repositoryProducts = repositoryProducts;
            _repositoryUsers = repositoryUsers;

            //there will be another validation for UserId or ProductId which doesnt exist in database.
            RuleFor(x => x.Customer).NotEmpty().NotNull();
            RuleFor(x => x.Address).NotEmpty().NotNull();
            RuleFor(x => x.UserId).NotEmpty().NotNull();
            RuleFor(x => x.ProductIds).NotEmpty().NotNull();
        }

        public class Handler : IRequestHandler<Command, Result>
        {
            private readonly IOrdersRepository _repository;
            private readonly IMapper _mapper;

            public Handler(IOrdersRepository repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }

            public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
            {
                var entry = _mapper.Map<Order>(request);
                await _repository.Add(entry, cancellationToken);
                //to change
                return new Result(entry.Id);
            }
        }

        public record Result(Guid id);
    }
}


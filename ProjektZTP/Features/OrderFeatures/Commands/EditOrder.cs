using FluentValidation;
using MediatR;
using ProjektZTP.Repository.Interfaces;

namespace ProjektZTP.Features.OrderFeatures.Commands;
public class EditOrder
{
    public record Command(
        Guid Id,
        string Address,
        string Customer
    ) : IRequest<Result>;

    public class Validator : AbstractValidator<EditData>
    {
        public Validator()
        {
            RuleFor(x => x.Address).NotEmpty().MaximumLength(20);
            RuleFor(x => x.Customer).NotEmpty().MaximumLength(15);
        }
    }

    public class Handler : IRequestHandler<Command, Result>
    {
        private readonly IOrdersRepository _repository;

        public Handler(IOrdersRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            var orderToEdit = await _repository.Get(request.Id, cancellationToken);

            orderToEdit.Address = request.Address;
            orderToEdit.Customer = request.Customer;

            var result = await _repository.Update(orderToEdit,cancellationToken);

            return new Result(
                result.Id,
                result.Address,
                result.Customer,
                result.UserId);
        }
    }

    public record Result(
        Guid Id,
        string Address,
        string Customer,
        Guid UserId);

    public record EditData(
        string Address,
        string Customer);
}


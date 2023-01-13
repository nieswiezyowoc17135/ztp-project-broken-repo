using FluentValidation;
using MediatR;
using ProjektZTP.Repository.Interfaces;

namespace ProjektZTP.Features.ProductFeatures.Commands;

public class EditProduct
{
    public record Command(
        Guid Id,
        string Name,
        float Price,
        int Amount,
        float Vat) : IRequest<Result>;

    public class Validator : AbstractValidator<EditData>
    {
        public Validator()
        {
            RuleFor(x => x.Name).NotEmpty().NotNull().MaximumLength(15);
            RuleFor(x => x.Price).NotEmpty().NotNull();
            RuleFor(x => x.Amount).NotEmpty().NotNull();
            RuleFor(x => x.Vat).NotEmpty().NotNull();
        }
    }

    public class Handler : IRequestHandler<Command, Result>
    {
        private readonly IProductsRepository _repository;

        public Handler(IProductsRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
        {
            var productToEdit = await _repository.Get(command.Id, cancellationToken);
            
            productToEdit.Name = command.Name;
            productToEdit.Price = command.Price;
            productToEdit.Amount = command.Amount;
            productToEdit.Vat = command.Vat;

            var result = await _repository.Update(productToEdit, cancellationToken);

            return new Result(
                result.Name,
                result.Price,
                result.Amount,
                result.Vat);
        }
    }

    public record Result(
        string Name,
        float Price,
        int Amount,
        float Vat);

    public record EditData(
        string Name,
        float Price,
        int Amount,
        float Vat);
}


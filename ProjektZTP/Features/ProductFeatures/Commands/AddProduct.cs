using FluentValidation;
using MediatR;
using NuGet.Packaging.Core;
using ProjektZTP.Models;
using ProjektZTP.Repository.Interfaces;

namespace ProjektZTP.Features.ProductFeatures.Commands;

public class AddProduct
{
    public record Command(
        string Name,
        float Price,
        int Amount,
        float Vat) : IRequest<Result>;

    public class Validator : AbstractValidator<Command>
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

        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            var entry = new Product(
                request.Name,
                request.Price,
                request.Amount,
                request.Vat);
            await _repository.Add(entry, cancellationToken);
            return new Result(entry.Id);
        }
    }

    public record Result(Guid id);
}


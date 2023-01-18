using FluentValidation;
using ProjektZTP.Models;
using ProjektZTP.Repository.Interfaces;
using static ProjektZTP.Mediator.Abstract;

namespace ProjektZTP.Features.ProductFeatures.Commands;

public class AddProduct
{
    public record AddProductCommand(
        string Name,
        float Price,
        int Amount,
        float Vat) : IRequest<AddProductResult>;

    public class Validator : AbstractValidator<AddProductCommand>
    {
        public Validator()
        {
            RuleFor(x => x.Name).NotEmpty().NotNull().MaximumLength(15);
            RuleFor(x => x.Price).NotEmpty().NotNull();
            RuleFor(x => x.Amount).NotEmpty().NotNull();
            RuleFor(x => x.Vat).NotEmpty().NotNull();
        }
    }

    public class AddProductCommandHandler : IHandler<AddProductCommand, AddProductResult>
    {
        private readonly IProductsRepository _repository;
        private Logger _logger;
        
        
        public AddProductCommandHandler(IProductsRepository repository)
        {
            _logger = Logger.GetInstance();
            _repository = repository;
        }

        public async Task<AddProductResult> HandleAsync(AddProductCommand request)
        {
            var validator = new Validator();

            var result = validator.Validate(request);

            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }

            var entry = new Product(
                request.Name,
                request.Price,
                request.Amount,
                request.Vat);

            await _repository.Add(entry);
            _logger.Log("Product created.");
            return new AddProductResult(entry.Id);
        }
    }

    public record AddProductResult(Guid id);
}


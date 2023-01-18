using FluentValidation;
using ProjektZTP.Repository.Interfaces;
using static ProjektZTP.Mediator.Abstract;

namespace ProjektZTP.Features.ProductFeatures.Commands;

public class EditProduct
{
    public record EditProductCommand(
        Guid Id,
        string Name,
        float Price,
        int Amount,
        float Vat) : IRequest<EditProductResult>;

    public class Validator : AbstractValidator<EditProductCommand>
    {
        public Validator()
        {
            RuleFor(x => x.Name).NotEmpty().NotNull().MaximumLength(15);
            RuleFor(x => x.Price).NotEmpty().NotNull();
            RuleFor(x => x.Amount).NotEmpty().NotNull();
            RuleFor(x => x.Vat).NotEmpty().NotNull();
        }
    }

    public class EditProductCommandHandler : IHandler<EditProductCommand, EditProductResult>
    { 
        private readonly IProductsRepository _repository;
        private Logger _logger;
        
        
        public EditProductCommandHandler(IProductsRepository repository)
        {
            _logger = Logger.GetInstance();
            _repository = repository;
        }

        public async Task<EditProductResult> HandleAsync(EditProductCommand request)
        {
            var validator = new Validator();

            var validatorResult = validator.Validate(request);

            if (!validatorResult.IsValid)
            {
                throw new ValidationException(validatorResult.Errors);
            }

            var productToEdit = await _repository.Get(request.Id);

            productToEdit.Name = request.Name;
            productToEdit.Price = request.Price;
            productToEdit.Amount = request.Amount;
            productToEdit.Vat = request.Vat;

            var result = await _repository.Update(productToEdit);
            _logger.Log("Product edited.");

            return new EditProductResult(
                result.Name,
                result.Price,
                result.Amount,
                result.Vat);
        }
    }

    public record EditProductResult(
        string Name,
        float Price,
        int Amount,
        float Vat);
}


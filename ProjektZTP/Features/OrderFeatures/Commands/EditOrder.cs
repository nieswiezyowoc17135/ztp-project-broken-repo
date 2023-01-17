using FluentValidation;
using ProjektZTP.Repository.Interfaces;
using static ProjektZTP.Mediator.Abstract;

namespace ProjektZTP.Features.OrderFeatures.Commands;
public class EditOrder
{
    public record EditOrderCommand(
        Guid Id,
        string Address,
        string Customer
    ) : IRequest<EditOrderResult>;

    public class Validator : AbstractValidator<EditOrderCommand>
    {
        public Validator()
        {
            RuleFor(x => x.Address).NotEmpty().MaximumLength(20);
            RuleFor(x => x.Customer).NotEmpty().MaximumLength(15);
        }
    }

    public class EditOrderCommandHandler : IHandler<EditOrderCommand, EditOrderResult>
    {
        private readonly IOrdersRepository _repository;

        private Logger _logger;
        
        
        public EditOrderCommandHandler(IOrdersRepository repository)
        {
            _logger = Logger.GetInstance();
            _repository = repository;
        }

        public async Task<EditOrderResult> HandleAsync(EditOrderCommand request)
        {
            var validator = new Validator();

            var validatorResult = validator.Validate(request);

            if (!validatorResult.IsValid)
            {
                throw new ValidationException(validatorResult.Errors);
            }

            var orderToEdit = await _repository.Get(request.Id);

            orderToEdit.Address = request.Address;
            orderToEdit.Customer = request.Customer;

            var result = await _repository.Update(orderToEdit);
            _logger.Log("Order edited.");
            return new EditOrderResult(
                result.Id,
                result.Address,
                result.Customer,
                result.UserId);
        }
    }

    public record EditOrderResult(
        Guid Id,
        string Address,
        string Customer,
        Guid UserId);
}


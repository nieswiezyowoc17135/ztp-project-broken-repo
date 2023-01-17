using FluentValidation;
using ProjektZTP.Models;
using ProjektZTP.Repository.Interfaces;
using static ProjektZTP.Mediator.Abstract;

namespace ProjektZTP.Features.UserFeatures.Commands;

public class AddUser
{
    public record AddUserCommand(
        string Login,
        string Password,
        string Email,
        string FirstName,
        string LastName) : IRequest<AddUserResult>;

    public class Validator : AbstractValidator<AddUserCommand>
    {
        public Validator()
        {
            RuleFor(x => x.Login).NotEmpty().NotNull().MaximumLength(15);
            RuleFor(x => x.Password).NotEmpty().NotNull().MaximumLength(15);
            RuleFor(x => x.Email).NotEmpty().NotNull().MaximumLength(20);
            RuleFor(x => x.FirstName).NotEmpty().NotNull().MaximumLength(15);
            RuleFor(x => x.LastName).NotEmpty().NotNull().MaximumLength(15);
        }
    }

    public class AddUserCommandHandler : IHandler<AddUserCommand, AddUserResult>
    {
        private readonly IUserRepository _repository;

        private Logger _logger;
        
        
        public AddUserCommandHandler(IUserRepository repository)
        {
            _logger = Logger.GetInstance();
            _repository = repository;
        }

        public async Task<AddUserResult> HandleAsync(AddUserCommand request)
        {
            var validator = new Validator();

            var result = validator.Validate(request);

            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }

            var entry = new User(
                request.Login,
                request.Password,
                request.Email,
                request.FirstName,
                request.LastName
);
            await _repository.Add(entry);
            _logger.Log("User created.");
            return new AddUserResult(entry.Id);
        }
    }

    public record AddUserResult(Guid Id);
}


using FluentValidation;

using ProjektZTP.Repository.Interfaces;
using static ProjektZTP.Mediator.Abstract;

namespace ProjektZTP.Features.UserFeatures.Commands
{
    public class EditUser
    {
        public record EditUserCommand(
            Guid Id,
            string Login,
            string Password,
            string Email,
            string FirstName,
            string LastName) : IRequest<EditUserResult>;

        public class Validator : AbstractValidator<EditUserCommand>
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

        public class EditUserCommandHandler : IHandler<EditUserCommand, EditUserResult>
        {
            private readonly IUserRepository _repository;
            private Logger _logger;
            
        
            public EditUserCommandHandler(IUserRepository repository)
            {
                _repository = repository;
                _logger = Logger.GetInstance();
            }

            public async Task<EditUserResult> HandleAsync(EditUserCommand command)
            {
                var validator = new Validator();

                var validatorResult = validator.Validate(command);

                if (!validatorResult.IsValid)
                {
                    throw new ValidationException(validatorResult.Errors);
                }


                var userToEdit = await _repository.Get(command.Id);

                userToEdit.FirstName = command.FirstName;
                userToEdit.LastName = command.LastName;
                userToEdit.Email = command.Email;
                userToEdit.Login = command.Login;
                userToEdit.Password = command.Password;

                var result = await _repository.Update(userToEdit);
                _logger.Log("User edited.");
                return new EditUserResult(
                    result.Login,
                    result.Password,
                    result.Email,
                    result.FirstName,
                    result.LastName);
            }
        }

        public record EditUserResult(
            string Login,
            string Password,
            string Email,
            string FirstName,
            string LastName);
    }
}

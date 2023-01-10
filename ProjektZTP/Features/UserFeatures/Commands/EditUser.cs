using FluentValidation;
using MediatR;
using ProjektZTP.Repository.Interfaces;

namespace ProjektZTP.Features.UserFeatures.Commands
{
    public class EditUser
    {
        public record Command(
            Guid Id,
            string Login,
            string Password,
            string Email,
            string FirstName,
            string LastName) : IRequest<Result>;

        public class Validator : AbstractValidator<EditData>
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

        public class Handler : IRequestHandler<Command, Result>
        {
            private readonly IUserRepository _repository;

            public Handler(IUserRepository repository)
            {
                _repository = repository;
            }

            public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
            {
                var userToEdit = await _repository.Get(command.Id, cancellationToken);

                userToEdit.FirstName = command.FirstName;
                userToEdit.LastName = command.LastName;
                userToEdit.Email = command.Email;
                userToEdit.Login = command.Login;
                userToEdit.Password = command.Password;

                var result = await _repository.Update(userToEdit, cancellationToken);

                return new Result(
                    result.Login,
                    result.Password,
                    result.Email,
                    result.FirstName,
                    result.LastName);
            }
        }

        public record Result(
            string Login,
            string Password,
            string Email,
            string FirstName,
            string LastName);

        public record EditData(
            string Login,
            string Password,
            string Email,
            string FirstName,
            string LastName);
    }
}

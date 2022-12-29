using FluentValidation;
using MediatR;
using ProjektZTP.Models;

namespace ProjektZTP.Features.UserFeatures
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

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.Id).NotEmpty().NotNull();
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

            public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
            {
                User dbSearchingResult = await _repository.Get(request.Id, cancellationToken);
                dbSearchingResult.FirstName = request.FirstName;
                dbSearchingResult.LastName = request.LastName;
                dbSearchingResult.Login = request.Login;
                dbSearchingResult.Password = request.Password;

                return new Result(
                    dbSearchingResult.Login,
                    dbSearchingResult.Password,
                    dbSearchingResult.Email,
                    dbSearchingResult.FirstName,
                    dbSearchingResult.LastName);
            }
        }

        public record Result(
            string Login,
            string Password,
            string Email,
            string FirstName,
            string LastName);
    }
}

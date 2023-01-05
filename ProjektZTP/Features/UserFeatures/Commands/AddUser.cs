using FluentValidation;
using MediatR;
using ProjektZTP.Models;
using ProjektZTP.Repository.Interfaces;

namespace ProjektZTP.Features.UserFeatures.Commands;

public class AddUser
{
    public record Command(
        string Login,
        string Password,
        string Email,
        string FirstName,
        string LastName) : IRequest<Result>;

    public class Validator : AbstractValidator<Command>
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

        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            var entry = new User(
                request.Login,
                request.Password,
                request.Email,
                request.FirstName,
                request.LastName
            );
            await _repository.Add(entry, cancellationToken);
            return new Result(entry.Id);
        }
    }

    public record Result(Guid Id);
}


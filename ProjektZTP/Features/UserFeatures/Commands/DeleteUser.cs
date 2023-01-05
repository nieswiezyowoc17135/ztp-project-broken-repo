using FluentValidation;
using MediatR;
using ProjektZTP.Repository.Interfaces;

namespace ProjektZTP.Features.UserFeatures.Commands;

public class DeleteUser
{
    public record Command(
        Guid Id) : IRequest<Result>;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Id).NotEmpty().NotNull();
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
            await _repository.Delete(request, cancellationToken);
            return new Result(request.Id);
        }
    }

    public record Result(
        Guid Id);
}
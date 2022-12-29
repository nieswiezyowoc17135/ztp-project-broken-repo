using MediatR;
using ProjektZTP.Models;

namespace ProjektZTP.Features.UserFeatures;

public class AddUser
{
    public record Command(
        string Login,
        string Password,
        string Email,
        string FirstName,
        string LastName) : IRequest<Result>;

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


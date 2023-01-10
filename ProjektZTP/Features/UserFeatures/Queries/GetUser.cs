using FluentValidation;
using MediatR;
using ProjektZTP.Repository.Interfaces;

namespace ProjektZTP.Features.UserFeatures.Queries;

public class GetUser
{
    public record Query(Guid Id) : IRequest<Result>;

    public class Handler : IRequestHandler<Query, Result>
    {
        private readonly IUserRepository _repository;

        public Handler(IUserRepository repository)
        {
            _repository = repository;
        }
        public async Task<Result> Handle(Query request, CancellationToken cancellationToken)
        {
            var user = await _repository.Get(request.Id, cancellationToken);
            return new Result(
                user.Login,
                user.Password,
                user.Email,
                user.FirstName,
                user.LastName);
        }
    }

    public record Result(
        string Login,
        string Password,
        string Email,
        string FirstName,
        string LastName);
}


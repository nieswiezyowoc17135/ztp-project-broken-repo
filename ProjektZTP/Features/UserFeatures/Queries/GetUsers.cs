using MediatR;
using ProjektZTP.Models;
using ProjektZTP.Repository.Interfaces;

namespace ProjektZTP.Features.UserFeatures.Queries;

public class GetUsers
{
    public record Query() : IRequest<Result>;

    public class Handler : IRequestHandler<Query, Result>
    {
        private readonly IUserRepository _repository;
        public Handler(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result> Handle(Query request, CancellationToken cancellationToken)
        {
            var result = await _repository.GetUsers(cancellationToken);
            return new Result(result);
        }
    }

    public record Result(
        IEnumerable<User> Users
        );

}

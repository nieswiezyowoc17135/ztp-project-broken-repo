using ProjektZTP.Models;
using ProjektZTP.Repository.Interfaces;
using static ProjektZTP.Mediator.Abstract;

namespace ProjektZTP.Features.UserFeatures.Queries;

public class GetUser
{
    public record GetUserQuery(Guid Id) : IRequest<GetUserResult>;

    public class GetUserQueryHandler : IHandler<GetUserQuery, GetUserResult>
    {
        private readonly IUserRepository _repository;

        public GetUserQueryHandler(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<GetUserResult> HandleAsync(GetUserQuery request)
        {
            var user = await _repository.Get(request.Id);
            return new GetUserResult(
                user.Login,
                user.Password,
                user.Email,
                user.FirstName,
                user.LastName,
                user.Orders);
        }
    }

    public record GetUserResult(
        string Login,
        string Password,
        string Email,
        string FirstName,
        string LastName,
        IEnumerable<Order> Orders);
}


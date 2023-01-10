using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
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
            var tmpList = new List<UserDTO>();
            var result = await _repository.GetUsers(cancellationToken);
            foreach (var user in result)
            {
                var tmp = new UserDTO(user.Login, user.Password, user.Email, user.FirstName, user.LastName,
                    user.Orders);
                tmpList.Add(tmp);
            }
            return new Result(tmpList);
        }
    }

    public record Result(
        List<UserDTO> Users
        );

    public record UserDTO(
        string Login,
        string Password,
        string Email,
        string FirstName,
        string LastName,
        IEnumerable<Order> Orders);
}

using ProjektZTP.Models;
using ProjektZTP.Repository.Interfaces;
using static ProjektZTP.Mediator.Abstract;

namespace ProjektZTP.Features.UserFeatures.Queries;

public class GetUsers
{
    public record GetUsersQuery() : IRequest<GetUsersResult>;

    public class Handler : IHandler<GetUsersQuery, GetUsersResult>
    {
        private readonly IUserRepository _repository;
        public Handler(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<GetUsersResult> HandleAsync(GetUsersQuery request)
        {
            var tmpList = new List<UserDTO>();
            var result = await _repository.GetUsers();
            foreach (var user in result)
            {
                var tmp = new UserDTO(user.Login, user.Password, user.Email, user.FirstName, user.LastName,
                    user.Orders);
                tmpList.Add(tmp);
            }
            return new GetUsersResult(tmpList);
        }
    }

    public record GetUsersResult(
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

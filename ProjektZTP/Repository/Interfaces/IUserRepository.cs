using ProjektZTP.Features.UserFeatures.Commands;
using ProjektZTP.Models;

namespace ProjektZTP.Repository.Interfaces;

public interface IUserRepository
{
    Task Add(User userEntry, CancellationToken cancellationToken);
    Task Delete(Guid Id, CancellationToken cancellationToken);
    Task<User> Update(EditUser.Command command, CancellationToken cancellationToken);
    Task<User> Get(Guid Id, CancellationToken cancellationToken);
    Task<IEnumerable<User>> GetUsers(CancellationToken cancellationToken);
    Task Save(CancellationToken cancellationToken);
}
using ProjektZTP.Models;

namespace ProjektZTP.Repository.Interfaces;

public interface IUserRepository
{
    Task Add(User userEntry, CancellationToken cancellationToken);
    Task Delete(User user, CancellationToken cancellationToken);
    Task<User> Update(User user, CancellationToken cancellationToken);
    Task<User> Get(Guid id, CancellationToken cancellationToken);
    Task<IEnumerable<User>> GetUsers(CancellationToken cancellationToken);
}
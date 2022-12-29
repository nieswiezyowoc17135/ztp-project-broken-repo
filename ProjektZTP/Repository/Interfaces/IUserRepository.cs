using ProjektZTP.Models;

public interface IUserRepository
{
    Task Add(User userEntry, CancellationToken cancellationToken);
    Task Delete(Guid Id, CancellationToken cancellationToken);
    Task<User> Get(Guid Id, CancellationToken cancellationToken);
}


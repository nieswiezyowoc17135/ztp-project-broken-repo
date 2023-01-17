using ProjektZTP.Models;

namespace ProjektZTP.Repository.Interfaces;

public interface IUserRepository
{
    Task Add(User userEntry);
    Task Delete(User user);
    Task<User> Update(User user);
    Task<User> Get(Guid id);
    Task<IEnumerable<User>> GetUsers();
}
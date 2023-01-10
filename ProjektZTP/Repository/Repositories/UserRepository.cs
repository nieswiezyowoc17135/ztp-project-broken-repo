using Microsoft.EntityFrameworkCore;
using ProjektZTP.Data;
using ProjektZTP.Models;
using ProjektZTP.Repository.Interfaces;

namespace ProjektZTP.Repository.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DatabaseContext _context;

        public UserRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task Add(User userEntry, CancellationToken cancellationToken)
        {
            if (userEntry == null)
            {
                throw new ArgumentNullException(nameof(userEntry));
            }

            await _context.Users.AddAsync(userEntry, cancellationToken);
            await Save(cancellationToken);
        }

        public async Task Delete(User user, CancellationToken cancellationToken)
        {
            _context.Users.Remove(user);
            await Save(cancellationToken);
        }

        public async Task<User> Update(User user, CancellationToken cancellationToken)
        {
            await Save(cancellationToken);
            return user;
        }


        public async Task<User> Get(Guid id, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FindAsync(new object[] { id }, cancellationToken);
            if (user == null)
            {
                throw new Exception("There is no User with this ID in database");
            }
            return user;
        }

        public async Task<IEnumerable<User>> GetUsers(CancellationToken cancellationToken)
        {
            IEnumerable<User> users = await _context.Users
                .AsNoTracking()
                .Select(x => new User(
                    x.Id,
                    x.Login,
                    x.Password,
                    x.Email,
                    x.FirstName,
                    x.LastName,
                    x.Orders)).ToListAsync(cancellationToken);
            return users;
        }

        public async Task Save(CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}

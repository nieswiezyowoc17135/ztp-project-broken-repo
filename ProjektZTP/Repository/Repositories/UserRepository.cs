using ProjektZTP.Data;
using ProjektZTP.Models;

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
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task Delete(Guid Id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<User> Get(Guid Id, CancellationToken cancellationToken)
        {
            var tmp = await _context.Users.FindAsync(new object[] { Id }, cancellationToken);
            if (tmp == null)
            {
                throw new Exception("There is no User with this ID in database");
            }
            else
            {
                return tmp;
            }
        }
    }
}

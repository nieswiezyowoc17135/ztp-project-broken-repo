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
        }

        public async Task Delete(Guid Id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task Get(Guid Id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}

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

        public async Task Add(User userEntry)
        {
            if (userEntry == null)
            {
                throw new ArgumentNullException(nameof(userEntry));
            }

            await _context.Users.AddAsync(userEntry);
            await Save();
        }

        public async Task Delete(User user)
        {
            _context.Users.Remove(user);
            await Save();
        }

        public async Task<User> Update(User user)
        {
            await Save();
            return user;
        }


        public async Task<User> Get(Guid id)
        {
            var user = await _context.Users.FindAsync(new object[] { id });
            if (user == null)
            {
                throw new Exception("There is no User with this ID in database");
            }
            return user;
        }

        public async Task<IEnumerable<User>> GetUsers()
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
                    x.Orders)).ToListAsync();
            return users;
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}

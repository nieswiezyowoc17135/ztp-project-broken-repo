using FluentValidation;
using ProjektZTP.Data;
using ProjektZTP.Features.UserFeatures.Commands;
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

        public async Task<Guid> Delete(DeleteUser.Command command, CancellationToken cancellationToken)
        {
            User dbSearchingResult = await Get(command.Id, cancellationToken);
            _context.Users.Remove(dbSearchingResult);
            await Save(cancellationToken);
            return dbSearchingResult.Id;
        }

        public async Task<User> Update(EditUser.Command command, CancellationToken cancellationToken)
        {

            User dbSearchingResult = await Get(command.Id, cancellationToken);
            dbSearchingResult.FirstName = command.FirstName;
            dbSearchingResult.LastName = command.LastName;
            dbSearchingResult.Login = command.Login;
            dbSearchingResult.Password = command.Password;
            await Save(cancellationToken);
            return dbSearchingResult;
        }


        public async Task<User> Get(Guid Id, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FindAsync(new object[] { Id }, cancellationToken);
            if (user == null)
            {
                throw new Exception("There is no User with this ID in database");
            }
            return user;
        }

        public Task<IEnumerable<User>> GetUsers(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task Save(CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}

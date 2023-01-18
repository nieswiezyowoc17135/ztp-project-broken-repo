using ProjektZTP.Repository.Interfaces;
using static ProjektZTP.Mediator.Abstract;

namespace ProjektZTP.Features.UserFeatures.Commands;

public class DeleteUser
{
    public record DeleteUserCommand(
        Guid Id) : IRequest<DeleteUserResult>;

    public class DeleteUserCommmandHandler : IHandler<DeleteUserCommand, DeleteUserResult>
    {
        private readonly IUserRepository _repository;

        private Logger _logger;
        
        
        public DeleteUserCommmandHandler(IUserRepository repository)
        {
            _logger = Logger.GetInstance();
            _repository = repository;
        }

        public async Task<DeleteUserResult> HandleAsync(DeleteUserCommand request)
        {
            var userToEdit = await _repository.Get(request.Id);
            await _repository.Delete(userToEdit);
            _logger.Log("User deleted.");
            return new DeleteUserResult(request.Id);
        }
    }

    public record DeleteUserResult(
        Guid Id);
}
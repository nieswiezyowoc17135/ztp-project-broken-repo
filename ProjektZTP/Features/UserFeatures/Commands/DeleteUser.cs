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

        public DeleteUserCommmandHandler(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<DeleteUserResult> HandleAsync(DeleteUserCommand request)
        {
            var userToEdit = await _repository.Get(request.Id);
            await _repository.Delete(userToEdit);

            return new DeleteUserResult(request.Id);
        }
    }

    public record DeleteUserResult(
        Guid Id);
}
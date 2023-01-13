using MediatR;
using ProjektZTP.Repository.Interfaces;

namespace ProjektZTP.Features.OrderFeatures.Commands;

public class DeleteOrder
{
    public record Command(
        Guid Id) : IRequest<Result>;

    public class Handler : IRequestHandler<Command, Result>
    {
        private readonly IOrdersRepository _repository;

        public Handler(IOrdersRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            var orderToDelete = await _repository.Get(request.Id, cancellationToken);
            await _repository.Delete(orderToDelete, cancellationToken);
            return new Result(request.Id);
        }
    }
    public record Result(Guid Id);
}


using ProjektZTP.Repository.Interfaces;
using static ProjektZTP.Mediator.Abstract;

namespace ProjektZTP.Features.OrderFeatures.Commands;

public class DeleteOrder
{
    public record DeleteOrderCommand(
        Guid Id) : IRequest<DeleteOrderResult>;

    public class DeleteOrderCommandHandler : IHandler<DeleteOrderCommand, DeleteOrderResult>
    {
        private readonly IOrdersRepository _repository;

        public DeleteOrderCommandHandler(IOrdersRepository repository)
        {
            _repository = repository;
        }

        public async Task<DeleteOrderResult> HandleAsync(DeleteOrderCommand request)
        {
            var orderToDelete = await _repository.Get(request.Id);
            await _repository.Delete(orderToDelete);

            return new DeleteOrderResult(request.Id);
        }
    }
    public record DeleteOrderResult(Guid Id);
}


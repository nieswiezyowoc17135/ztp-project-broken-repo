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

        private Logger _logger;
        
        public DeleteOrderCommandHandler(IOrdersRepository repository)
        {
            _logger = Logger.GetInstance();
            _repository = repository;
        }

        public async Task<DeleteOrderResult> HandleAsync(DeleteOrderCommand request)
        {
            var orderToDelete = await _repository.Get(request.Id);
            await _repository.Delete(orderToDelete);
            _logger.Log("Order deleted.");
            return new DeleteOrderResult(request.Id);
        }
    }
    public record DeleteOrderResult(Guid Id);
}


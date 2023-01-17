using ProjektZTP.Repository.Interfaces;
using static ProjektZTP.Mediator.Abstract;

namespace ProjektZTP.Features.OrderFeatures.Queries;

public class GetOrder
{
    public record GetOrderQuery(Guid id) : IRequest<GetOrderResult>;

    public class GetOrderQueryHandler : IHandler<GetOrderQuery, GetOrderResult>
    {
        private readonly IOrdersRepository _repository;

        public GetOrderQueryHandler(IOrdersRepository repository)
        {
            _repository = repository;
        }

        public async Task<GetOrderResult> HandleAsync(GetOrderQuery request)
        {
            var order = await _repository.Get(request.id);
            return new GetOrderResult(
                order.Id,
                order.Address,
                order.Customer,
                order.UserId);
        }
    }

    public record GetOrderResult(
        Guid Id,
        string Address,
        string Customer,
        Guid UserId);
}


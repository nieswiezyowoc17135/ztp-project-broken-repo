using ProjektZTP.Repository.Interfaces;
using static ProjektZTP.Mediator.Abstract;

namespace ProjektZTP.Features.OrderFeatures.Queries;

public class GetOrders
{
    public record GetOrdersQuery() : IRequest<GetOrdersResult>;

    public class GetOrdersQueryHandler : IHandler<GetOrdersQuery, GetOrdersResult>
    {
        private readonly IOrdersRepository _repository;

        public GetOrdersQueryHandler(IOrdersRepository repository)
        {
            _repository = repository;
        }

        public async Task<GetOrdersResult> HandleAsync(GetOrdersQuery request)
        {
            var tmpList = new List<OrderDTO>();
            var result = await _repository.GetOrders();

            foreach (var order in result)
            {
                var tmp = new OrderDTO(
                    order.Id,
                    order.Customer,
                    order.Address,
                    order.UserId);
                tmpList.Add(tmp);
            }

            return new GetOrdersResult(tmpList);
        }
    }

    public record GetOrdersResult(
        List<OrderDTO> Orders);

    public record OrderDTO(
        Guid Id,
        string Customer,
        string Address,
        Guid UserId);
}


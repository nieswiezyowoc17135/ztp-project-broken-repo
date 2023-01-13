using MediatR;
using ProjektZTP.Repository.Interfaces;

namespace ProjektZTP.Features.OrderFeatures.Queries;

public class GetOrders
{
    public record Query() : IRequest<Result>;

    public class Handler : IRequestHandler<Query, Result>
    {
        private readonly IOrdersRepository _repository;

        public Handler(IOrdersRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result> Handle(Query request, CancellationToken cancellationToken)
        {
            var tmpList = new List<OrderDTO>();
            var result = await _repository.GetOrders(cancellationToken);

            foreach (var order in result)
            {
                var tmp = new OrderDTO(
                    order.Id,
                    order.Customer,
                    order.Address,
                    order.UserId);
                tmpList.Add(tmp);
            }

            return new Result(tmpList);
        }
    }

    public record Result(
        List<OrderDTO> Orders);

    public record OrderDTO(
        Guid Id,
        string Customer,
        string Address,
        Guid UserId);
}


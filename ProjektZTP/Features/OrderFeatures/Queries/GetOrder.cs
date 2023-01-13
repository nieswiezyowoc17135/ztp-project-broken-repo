using MediatR;
using ProjektZTP.Repository.Interfaces;

namespace ProjektZTP.Features.OrderFeatures.Queries;

public class GetOrder
{
    public record Query(Guid id) : IRequest<Result>;

    public class Handler : IRequestHandler<Query, Result>
    {
        private readonly IOrdersRepository _repository;

        public Handler(IOrdersRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result> Handle(Query request, CancellationToken cancellationToken)
        {
            var order = await _repository.Get(request.id, cancellationToken);
            return new Result(
                order.Id,
                order.Address,
                order.Customer,
                order.UserId);
        }
    }

    public record Result(
        Guid Id,
        string Address,
        string Customer,
        Guid UserId);
}


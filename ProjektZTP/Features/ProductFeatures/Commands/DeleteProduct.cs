using MediatR;
using ProjektZTP.Repository.Interfaces;

namespace ProjektZTP.Features.ProductFeatures.Commands;

public class DeleteProduct
{
    public record Command(
        Guid Id) : IRequest<Result>;

    public class Handler : IRequestHandler<Command, Result>
    {
        private readonly IProductsRepository _repository;

        public Handler(IProductsRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            var productToDelete = await _repository.Get(request.Id, cancellationToken);
            await _repository.Delete(productToDelete, cancellationToken);
            return new Result(request.Id);
        }
    }

    public record Result(Guid Id);
}


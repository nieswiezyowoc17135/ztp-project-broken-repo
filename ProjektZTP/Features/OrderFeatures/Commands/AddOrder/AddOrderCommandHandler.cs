using ProjektZTP.Data;
using static ProjektZTP.Mediator.Abstract;


namespace ProjektZTP.Features.OrderFeatures.Commands.AddOrder
{
    public class AddOrderCommandHandler : IHandler<AddOrderCommand, AddOrderCommandResult>
    {
        private readonly DatabaseContext _context;

        public AddOrderCommandHandler(DatabaseContext context)
        {
            _context = context;
        }

        public Task<AddOrderCommandResult> HandleAsync(AddOrderCommand request)
        {
            var a = _context.Orders.ToList();

            return Task.FromResult(new AddOrderCommandResult());
        }
    }
}

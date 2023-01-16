using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using ProjektZTP.Data;
using static ProjektZTP.Mediator.MediatorPattern;

namespace ProjektZTP.Features.OrderFeatures.Commands.AddOrder
{
    public class AddOrderCommandHandler : IRequestHandler<AddOrderCommand, AddOrderCommandResult>
    {
        private readonly DatabaseContext _context;

        public AddOrderCommandHandler(DatabaseContext context)
        {
            _context = context;
        }

        public AddOrderCommandResult Handle(AddOrderCommand request)
        {
            var a = _context.Orders.ToList();

            return new AddOrderCommandResult();
        }
    }
}


using static ProjektZTP.Mediator.MediatorPattern;

namespace ProjektZTP.Features.OrderFeatures.Commands.AddOrder
{
    public class AddOrderCommand : IRequest<AddOrderCommandResult>
    {
        public string Text { get; set; }
    }
}

using System.Collections.Concurrent;
using System.Threading.Tasks;
using static ProjektZTP.Mediator.Abstract;

namespace ProjektZTP.Mediator
{
    public class MediatorPattern
    {
        public interface IMediator
        {
            Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request);
        }

        public class Mediator : IMediator
        {
            private readonly IServiceProvider _serviceProvider;
            private readonly IDictionary<Type, Type> _handlerDetails;

            public Mediator(IServiceProvider serviceProvider, IDictionary<Type, Type> handlerDetails)
            {
                _serviceProvider = serviceProvider;
                _handlerDetails = handlerDetails;
            }

            public async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
            {
                var requestType = request.GetType();
                if (!_handlerDetails.ContainsKey(requestType))
                {
                    throw new Exception($"No handler {requestType.Name}");
                }

                _handlerDetails.TryGetValue(requestType, out var requestHandlerType);
                var handler = _serviceProvider.GetRequiredService(requestHandlerType);

                return await (Task<TResponse>)handler.GetType().GetMethod("HandleAsync")!.Invoke(handler, new[] { request });
            }
        }
    }
}


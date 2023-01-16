namespace ProjektZTP.Mediator
{
    public class MediatorPattern
    {
        public interface ICustomMediator
        {
            TResponse Send<TResponse>(IRequest<TResponse> request);
        }

        public interface IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
        {
            TResponse Handle(TRequest request);
        }

        public interface IRequest<TResponse>
        {
        }

        public class Mediator : ICustomMediator
        {
            private readonly IDictionary<Type, object> _handlers;

            public Mediator()
            {
                _handlers = new Dictionary<Type, object>();
            }

            public void Register<TRequest, TResponse, THandler>()
                where TRequest : IRequest<TResponse>
                where THandler : IRequestHandler<TRequest, TResponse>
            {
                _handlers.Add(typeof(TRequest), Activator.CreateInstance<IRequestHandler<TRequest, TResponse>>());
            }

            public TResponse Send<TResponse>(IRequest<TResponse> request)
            {
                var requestType = request.GetType();
                object handler;
                if (!_handlers.TryGetValue(requestType, out handler))
                {
                    throw new Exception($"No handler registered for request of type {requestType}");
                }
                var castedHandler = (IRequestHandler<IRequest<TResponse>, TResponse>)handler;
                return castedHandler.Handle((dynamic)request);
            }
        }
    }
}

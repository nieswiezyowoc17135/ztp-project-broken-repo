namespace ProjektZTP.Mediator
{
    public class Abstract
    {
        public interface IRequest<TResponse>
        {
        }

        public interface IHandler<in TRequest, TResponse> where TRequest : IRequest<TResponse>
        {
            Task<TResponse> HandleAsync(TRequest request);
        }
    }
}

using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;
using static ProjektZTP.Mediator.Abstract;
using static ProjektZTP.Mediator.MediatorPattern;

namespace ProjektZTP.Mediator
{
    public static class MediatorExtensions
    {
        public static IServiceCollection AddMediator(this IServiceCollection services, ServiceLifetime lifetime, params Type[] markers)
        {
            var handlerInfo = new Dictionary<Type, Type>();

            foreach (var marker in markers)
            {
                var assembly = marker.Assembly;
                var requests = GetClassesImplementingInterface(assembly, typeof(IRequest<>));
                var handlers = GetClassesImplementingInterface(assembly, typeof(IHandler<,>));

                requests.ForEach(x =>
                {
                    handlerInfo[x] = handlers.SingleOrDefault(xx => x == xx.GetInterface("IHandler`2")!.GetGenericArguments()[0]);
                });

                var serviceDescriptor = handlers.Select(x => new ServiceDescriptor(x, x, lifetime));
                services.TryAdd(serviceDescriptor);
            }

            services.AddScoped<IMediator>(x => new MediatorPattern.Mediator(x, handlerInfo));

            return services;
        }

        private static List<Type> GetClassesImplementingInterface(Assembly assembly, Type typeToMatch)
        {
            return assembly.ExportedTypes
                .Where(type =>
                {
                    var genericInterfaceTypes = type.GetInterfaces().Where(x => x.IsGenericType).ToList();
                    var implementerRequestType = genericInterfaceTypes
                        .Any(x => x.GetGenericTypeDefinition() == typeToMatch);
                    return !type.IsInterface && !type.IsAbstract && implementerRequestType;
                }).ToList();
        }
    }
}

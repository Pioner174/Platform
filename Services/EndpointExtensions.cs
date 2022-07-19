using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Platform.Services;
using System.Reflection;
using System.Threading.Tasks;
using System.Linq;

namespace Microsoft.AspNetCore.Builder
{
    public static class EndpointExtensions
    {
        public static void MapEndpoint<T>(this IEndpointRouteBuilder app, string path, string methodName = "Endpoint")
        {
            MethodInfo methodInfo = typeof(T).GetMethod(methodName);

            if(methodInfo == null || methodInfo.ReturnType != typeof(Task))
            {
                throw new System.Exception("Method cannot be used");
            }

            T endpointInstance = ActivatorUtilities.CreateInstance<T>(app.ServiceProvider);

            //app.Map(path, (RequestDelegate)methodInfo.CreateDelegate(typeof(RequestDelegate), endpointInstance));

            ParameterInfo[] parameters = methodInfo.GetParameters();
            app.Map(path, context =>
            {
                T endPointInstance = ActivatorUtilities.CreateInstance<T>(context.RequestServices);
                return (Task)methodInfo.Invoke(endpointInstance, parameters.Select(p => p.ParameterType == typeof(HttpContext) ?
            context : context.RequestServices.GetService(p.ParameterType)).ToArray());
        }); 
        }
    }
}



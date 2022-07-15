using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Platform.Services;

namespace Platform
{
    public class WeatherMiddleware
    {
        RequestDelegate next;

        IResponseFormatter formatter;
        public WeatherMiddleware(RequestDelegate next, IResponseFormatter respformatter) : this(next)
        {
            formatter = respformatter;
        }

        public WeatherMiddleware(RequestDelegate requestDelegate)
        {
            next = requestDelegate;
        }

        public async Task Invoke(HttpContext context)
        {
            if(context.Request.Path == "/middleware/class")
            {
                await formatter.Format(context,"Middleware Class: It is raining in London");
            }else
                await next(context);
        }
    }
}

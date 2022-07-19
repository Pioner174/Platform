using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Platform.Services;

namespace Platform
{
    public class WeatherMiddleware
    {
        RequestDelegate next;

        //IResponseFormatter formatter;
        public WeatherMiddleware(RequestDelegate nextDelegate) 
        {
            next = nextDelegate;
        }


        public async Task Invoke(HttpContext context, IResponseFormatter formatter, IResponseFormatter formatter2, IResponseFormatter formatter3)
        {
            if(context.Request.Path == "/middleware/class")
            {
                await formatter.Format(context,string.Empty);
                await formatter2.Format(context, string.Empty);
                await formatter3.Format(context, string.Empty);
            }
            else
                await next(context);
        }
    }
}

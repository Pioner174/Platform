using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Platform.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Platform
{
    public class WeatherEndpoint
    {
        IResponseFormatter formatter;

        public WeatherEndpoint(IResponseFormatter respFormatter)
        {
            formatter = respFormatter;
        }

        public  async Task Endpoint(HttpContext context)
        {
            await formatter.Format(context,"Endpoint Class: It is cloudy in Milan");
        }
    }
}

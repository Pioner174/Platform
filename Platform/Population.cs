using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Platform
{
    public class Population
    {
        private RequestDelegate next;

        public Population() { }

        public Population(RequestDelegate nextDelegate)
        {
            next = nextDelegate;
        }

        public async Task Invoke(HttpContext context)
        {
            string[] parts = context.Request.Path.ToString().Split("/", StringSplitOptions.RemoveEmptyEntries);

            if(parts.Length == 2 && parts[0] == "population")
            {
                string city = parts[1];

                int? pop = null;
                switch (city.ToLower())
                {
                    case "london":
                        pop = 8_136_000;
                        break;
                    case "paris":
                        pop = 2141000;
                        break;
                    case "monako":
                        pop = 39000;
                        break;
                }
                if (pop.HasValue)
                {
                    await context.Response.WriteAsync($"City: {city}, Population: {pop}");
                    return;
                }
            }
            if(next != null)
            {
                await next(context);
            }
        }
    }
}

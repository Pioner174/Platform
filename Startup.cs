using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Platform
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<MessageOptions>(opts => { opts.CityName = "Albany"; });
            services.Configure<RouteOptions>(opts =>
            {
                opts.ConstraintMap.Add("countryName", typeof(CountryRouteConstraint));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            
            app.UseDeveloperExceptionPage();


            //app.UseMiddleware<Population>();
            //app.UseMiddleware<Capital>();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.Map("{number:int}", async context =>
                {
                    await context.Response.WriteAsync("Routed to the int endpoint");
                }).Add(b => ((RouteEndpointBuilder)b).Order = 1);//установка приоритета для конечной точки

                endpoints.Map("{number:double}", async context =>
                {
                    await context.Response.WriteAsync("Routed to the double endpoint");
                }).Add(b=>((RouteEndpointBuilder)b).Order=2);

                endpoints.Map("{first:alpha:length(3)}/{second:bool}", async context =>
                {
                    await context.Response.WriteAsync("Request was routed\n");
                    foreach (var kvp in context.Request.RouteValues)
                    {
                        await context.Response.WriteAsync($"{kvp.Key}:{kvp.Value}\n");
                    }
                });

                endpoints.MapGet("capital/{country:countryName}", Capital.Endpoint);
                endpoints.Map("size/{city?}", Population.Endpoint).WithMetadata(new RouteNameMetadata("population"));
                endpoints.MapFallback(async context =>
                {
                    await context.Response.WriteAsync("Routed to fallback endpoint");
                });
            });

            app.Use(async (context, next) => await context.Response.WriteAsync("Terminal middleware reached"));

        }
    }
}

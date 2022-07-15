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
using Platform.Services;

namespace Platform
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IResponseFormatter, HtmlResponseFormatter>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IResponseFormatter formatter)
        {
            
            app.UseDeveloperExceptionPage();


            //app.UseMiddleware<Population>();
            //app.UseMiddleware<Capital>();

            app.UseRouting();

            app.UseMiddleware<WeatherMiddleware>();

            app.Use(async (context, next) =>
            {
                if (context.Request.Path == "/middleware/function")
                {
                    await formatter.Format(context, "Middleware Function: It is snowing in Chicago");
                }
                else
                    await next();
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.Map("/endpoint/class", WeatherEndpoint.Endpoint);
                endpoints.Map("/endpoint/function", async context =>
                {
                    await formatter.Format(context,"Endpoint Function: It is sunny in LA");
                });
            });

            

        }
    }
}

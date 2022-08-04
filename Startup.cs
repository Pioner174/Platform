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
using Microsoft.Extensions.Configuration;
using Platform.Services;

namespace Platform
{
    public class Startup
    {
        private IConfiguration Configuration { get; set; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<MessageOptions>(Configuration.GetSection("Location"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseMiddleware<LocationMiddleware>();

            app.Use(async (context, next) =>
            {
                string defaultDebug = Configuration["Logging:LogLevel:Default"];
                await context.Response.WriteAsync($"The config setting is: {defaultDebug}");
                string environ = Configuration["ASPNETCORE_ENVIRONMENT"];
                await context.Response.WriteAsync($"\nTHE ENV SETTING IS: {environ}");
                string wsID = Configuration["WebService:Id"];
                string wsKey = Configuration["WebService:Key"];
                await context.Response.WriteAsync($"\nTHE secret id IS: {wsID}");
                await context.Response.WriteAsync($"\nTHE secret Key IS: {wsKey}");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.Map("/", async context =>
                {
                    await context.Response.WriteAsync("\nHello world!");
                });
            });

            

        }
    }
}

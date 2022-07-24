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

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration;
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(typeof(ICollection<>), typeof(List<>));

            //services.AddScoped<ITimeStamper, DefaultTimeStamper>();
            //services.AddScoped<IResponseFormatter, TextResponseFormatter>();
            //services.AddScoped<IResponseFormatter, GuidService>();
            //services.AddScoped<IResponseFormatter, TimeResponseFormatter>();
            //services.AddScoped<IResponseFormatter, HtmlResponseFormatter>();


            //services.AddScoped<IResponseFormatter>(serviceProvider =>
            //{
            //    string typeName = Configuration["services:IResponseFormatter"];
            //    return (IResponseFormatter)ActivatorUtilities.CreateInstance(serviceProvider,
            //        typeName == null ? typeof(GuidService) : Type.GetType(typeName, true));
            //});

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
                endpoints.Map("/string", async context =>
                {
                    ICollection<string> collection = context.RequestServices.GetService<ICollection<string>>();
                    collection.Add($"Request: {DateTime.Now.ToLongTimeString()}");
                    foreach (string str in collection)
                    {
                        await context.Response.WriteAsync($"String: {str}\n");
                    }
                });
                endpoints.Map("/int", async context =>
                {
                    ICollection<int> collection = context.RequestServices.GetService<ICollection<int>>();
                    collection.Add(collection.Count + 1);
                    foreach (int val in collection)
                    {
                        await context.Response.WriteAsync($"Int: {val}\n");
                    }
                });

                //endpoints.Map("/single", async context =>
                //{
                //    IResponseFormatter formatter = context.RequestServices.GetService<IResponseFormatter>();
                //    await formatter.Format(context, "SINGLE service");
                //});
                //endpoints.Map("/", async context =>
                //{
                //    IResponseFormatter formatter = context.RequestServices.GetServices<IResponseFormatter>().First(f=>f.RichOutput);
                //    await formatter.Format(context, "Multiple services");
                //});
            });


            //app.UseMiddleware<WeatherMiddleware>();

            //app.Use(async (context, next) =>
            //{
            //    if (context.Request.Path == "/middleware/function")
            //    {
            //        IResponseFormatter formatter = context.RequestServices.GetService<IResponseFormatter>();
            //        await formatter.Format(context, "Middleware Function: It is snowing in Chicago");
            //    }
            //    else
            //        await next();
            //});

            //app.UseEndpoints(endpoints =>
            //{
            //    //endpoints.Map("/endpoint/class", WeatherEndpoint.Endpoint);
            //    endpoints.MapEndpoint<WeatherEndpoint>("/endpoint/class");
            //    endpoints.Map("/endpoint/function", async context =>
            //    {
            //        IResponseFormatter formatter = context.RequestServices.GetService<IResponseFormatter>();
            //        await formatter.Format(context, "Endpoint Function: It is sunny in LA");
            //    });

            //});

            

        }
    }
}

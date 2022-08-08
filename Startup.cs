using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Platform
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(opts =>
            {
                opts.CheckConsentNeeded = context => true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();

            app.UseCookiePolicy();

            app.UseMiddleware<ConsentMiddleware>();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.Map("/cookie", async context =>
                {
                    int counter1 = int.Parse(context.Request.Cookies["counter1"] ?? "0")+1;
                    context.Response.Cookies.Append("counter1", counter1.ToString(), new CookieOptions { MaxAge = TimeSpan.FromMinutes(30), IsEssential = true }) ;

                    int counter2 = int.Parse(context.Request.Cookies["counter2"] ?? "0") + 1;
                    context.Response.Cookies.Append("counter2", counter2.ToString(), new CookieOptions { MaxAge = TimeSpan.FromMinutes(30) });

                    await context.Response.WriteAsync($"Counter1: {counter1}, Counter2: {counter2}");
                });
                endpoints.MapGet("clear", context =>
                {
                    context.Response.Cookies.Delete("counter1");
                    context.Response.Cookies.Delete("counter2");
                    context.Response.Redirect("/");
                    return Task.CompletedTask;
                });
                endpoints.MapFallback(async context => await context.Response.WriteAsync("Hello world!"));
            });



        }
    }
}

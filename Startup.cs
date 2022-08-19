using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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

            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.IsEssential = true;
            });

            services.AddHsts(opts =>
            {
                opts.MaxAge = TimeSpan.FromDays(1);
                opts.IncludeSubDomains = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();

            if (env.IsProduction())
            {
                app.UseHsts();
            }
            app.UseHttpsRedirection();

            app.UseCookiePolicy();

            app.UseMiddleware<ConsentMiddleware>();

            app.UseSession();

            app.UseRouting();

            app.Use(async (context, next) =>
            {
                await context.Response.WriteAsync($"HTTPS Request: {context.Request.IsHttps}\n");
                await next();
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.Map("/cookie", async context =>
                {
                    /// Использование сеансов
                    int counter1 = (context.Session.GetInt32("counter1") ?? 0) + 1;
                    int counter2 = (context.Session.GetInt32("counter2") ?? 0) + 1;

                    context.Session.SetInt32("counter1", counter1);
                    context.Session.SetInt32("counter2", counter2);

                    await context.Session.CommitAsync();
                    await context.Response.WriteAsync($"Counter1: {counter1}, Counter2: {counter2}");

                    /// Использование ckookie
                    //int counter1 = int.Parse(context.Request.Cookies["counter1"] ?? "0")+1;
                    //context.Response.Cookies.Append("counter1", counter1.ToString(), new CookieOptions { MaxAge = TimeSpan.FromMinutes(30), IsEssential = true }) ;

                    //int counter2 = int.Parse(context.Request.Cookies["counter2"] ?? "0") + 1;
                    //context.Response.Cookies.Append("counter2", counter2.ToString(), new CookieOptions { MaxAge = TimeSpan.FromMinutes(30) });

                    //await context.Response.WriteAsync($"Counter1: {counter1}, Counter2: {counter2}");
                });
                /// Отчистка ckookie
                //endpoints.MapGet("clear", context =>
                //{
                //    context.Response.Cookies.Delete("counter1");
                //    context.Response.Cookies.Delete("counter2");
                //    context.Response.Redirect("/");
                //    return Task.CompletedTask;
                //});
                endpoints.MapFallback(async context => await context.Response.WriteAsync("Hello world!"));
            });



        }
    }
}

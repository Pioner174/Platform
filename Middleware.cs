﻿using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace Platform
{
    public class QueryStringMiddleWare
    {
        private RequestDelegate next;

        public QueryStringMiddleWare()
        {

        }

        public QueryStringMiddleWare(RequestDelegate nextDelegate)
        {
            next = nextDelegate;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Method == "GET" && context.Request.Query["custom"] == "true")
            {
                await context.Response.WriteAsync("Class-based Middleware\n");
            }
            if (next != null)
            {
                await next(context);
            }
        }
    }

    public class LocationMiddleware
    {
        private RequestDelegate next;
        private MessageOptions options;

        public LocationMiddleware(RequestDelegate nextde, IOptions<MessageOptions> opts)
        {
            next = nextde;
            options = opts.Value;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path == "/location")
            {
                await context.Response.WriteAsync($"{options.CityName}, {options.CountryName}");
            }
            else
            {
                await next(context);
            }
                
        }
    }
}

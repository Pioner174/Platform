﻿using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

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
}

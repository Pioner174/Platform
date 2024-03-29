﻿using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Platform
{
    public class CountryRouteConstraint : IRouteConstraint // Интерфейс для создания специальных ограничений
    {
        private static string[] countries = { "uk", "france", "monaco", "russia" };
        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            string segmentValue = values[routeKey] as string ?? "";
            return Array.IndexOf(countries, segmentValue.ToLower()) > -1;
        }
    }
}

﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using System.Threading.Tasks;


namespace Platform
{
    public class ConsentMiddleware
    {
        private RequestDelegate _next;

        public ConsentMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path == "/consent")
            {
                ITrackingConsentFeature consentFeature = context.Features.Get<ITrackingConsentFeature>();

                if (!consentFeature.HasConsent)
                {
                    consentFeature.GrantConsent();
                }
                else
                {
                    consentFeature.WithdrawConsent();
                }
                await context.Response.WriteAsync(consentFeature.HasConsent ? "Consent Granted \n" : "Consent Withdrawn\n");
            }
            await _next(context);
        }
    }
}

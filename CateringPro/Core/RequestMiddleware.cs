using CateringPro.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CateringPro.Core
{
    public class RequestMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestMiddleware> _logger;
        private readonly string[] logpathes = { "Dishes", "UserGroups", "IngredientCategories", "Categories", "Docs", "DayDishes", "Complex", "MyOrders", "Account" };


        public RequestMiddleware(RequestDelegate next, ILogger<RequestMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            bool isLog = false;

            if (context.Request != null)
                isLog = context.Request.Path.ToString().Split("/").Any(s => logpathes.Contains(s));

            try
            {
                await _next(context);
            }
            finally
            {
                if (context.Response != null && context.Response.StatusCode != (int)HttpStatusCode.OK)
                    isLog = true;
                if (isLog)
                {
                    _logger.LogWarning($"Request {context.Request?.Method} {context.Request?.Path.Value} => {context.Response?.StatusCode}");
                }


            }
        }
    }
}

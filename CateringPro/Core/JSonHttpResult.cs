using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CateringPro.Core
{
    public class JsonHttpStatusResult : JsonResult
    {
        private readonly HttpStatusCode _httpStatus;

        public JsonHttpStatusResult(object data, HttpStatusCode httpStatus) : base(data)
        {

            _httpStatus = httpStatus;
        }

        public override Task ExecuteResultAsync(ActionContext context)
        {

            context.HttpContext.Response.StatusCode = (int)_httpStatus;
            return base.ExecuteResultAsync(context);
        }
    }
}

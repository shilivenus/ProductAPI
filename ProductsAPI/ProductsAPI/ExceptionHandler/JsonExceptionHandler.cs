using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using ProductsAPI.ExceptionHandler.Exception;
using System;
using System.Net;
using System.Threading.Tasks;

namespace ProductsAPI.ExceptionHandler
{
    public static class JsonExceptionHandler
    {
        public static Task HandleExceptionAsync(HttpContext context)
        {
            var ex = context.Features.Get<IExceptionHandlerFeature>()?.Error;
            Type type = ex?.GetType();

            if (type?.IsGenericType == true && type.GetGenericTypeDefinition() == typeof(ErrorCodeException<>))
            {
                string propertyName = nameof(ErrorCodeException<Enum>.Code);
                var errorCode = type.GetProperty(propertyName).GetValue(ex);
                var response = new { errorCode, message = ex.Message };

                return WriteResponseAsync(context, HttpStatusCode.BadRequest, response);
            }
            else
            {
                var response = new { message = ex?.Message ?? "An internal error has occurred." };

                return WriteResponseAsync(context, HttpStatusCode.InternalServerError, response);
            }
        }

        private static Task WriteResponseAsync(HttpContext context, HttpStatusCode httpStatusCode, object response)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)httpStatusCode;

            return context.Response.WriteAsync(JsonConvert.SerializeObject(response));
        }
    }
}

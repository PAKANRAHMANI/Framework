using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Framework.Core.Exceptions;
using Framework.AspNetCore.Configurations;
using Sentry;

namespace Framework.AspNetCore.MiddleWares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly ExceptionLogConfiguration _exceptionLogConfiguration;

        public ExceptionHandlerMiddleware(RequestDelegate next, ExceptionLogConfiguration exceptionLogConfiguration)
        {
            _next = next;
            _exceptionLogConfiguration = exceptionLogConfiguration;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception exception)
            {
                await HandleException(context, exception);
            }
        }

        private async Task HandleException(HttpContext context, Exception exception)
        {
            if (exception is BusinessException businessException)
                await HandleBusinessException(context, businessException);
            else await UnhandledException(context, exception);
        }

        private async Task HandleBusinessException(HttpContext context, BusinessException businessException)
        {
            var error = ExceptionDetails.Create(businessException.ExceptionMessage, businessException.ErrorCode);
            await WriteExceptionToResponse(context, error);
        }

        private async Task WriteExceptionToResponse(HttpContext context, ExceptionDetails error)
        {
            context.Response.StatusCode = (int)error.Code;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonConvert.SerializeObject(error));
        }

        private async Task UnhandledException(HttpContext httpContext, Exception exception)
        {
            if (IsUnhandledExceptionsCapturedBySentry())
                CaptureOnSentry(exception);

            var error = ExceptionDetails.Create(exception.Message, -1000);
            await WriteExceptionToResponse(httpContext, error);
        }

        private bool IsUnhandledExceptionsCapturedBySentry()
        {
            return _exceptionLogConfiguration.CaptureBySentry;
        }

        private void CaptureOnSentry(Exception exception)
        {
            using (SentrySdk.Init(_exceptionLogConfiguration.SentryConfiguration.Dsn))
            {
                SentrySdk.CaptureException(exception);
            }
        }
    }
}

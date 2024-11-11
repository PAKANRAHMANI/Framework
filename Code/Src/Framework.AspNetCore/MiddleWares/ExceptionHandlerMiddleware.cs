using Framework.AspNetCore.Configurations;
using Framework.Core;
using Framework.Core.Exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net;

namespace Framework.AspNetCore.MiddleWares;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ExceptionLogConfiguration _exceptionLogConfiguration;
    private readonly Core.Logging.ILogger _logger;

    public ExceptionHandlerMiddleware(RequestDelegate next, ExceptionLogConfiguration exceptionLogConfiguration, Core.Logging.ILogger logger)
    {
        _next = next;
        _exceptionLogConfiguration = exceptionLogConfiguration;
        _logger = logger;
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

            _logger.WriteException(exception);

            if (IsUnhandledExceptionsCapturedBySentry())
                CaptureOnSentry(exception);
        }
    }

    private async Task HandleException(HttpContext context, Exception exception)
    {
        switch (exception)
        {
            case BusinessException businessException:
                await HandleBusinessException(context, businessException);
                break;
            case InfrastructureException infrastructureException:
                await HandleInfrastructureException(context, infrastructureException);
                break;
            default:
                await UnhandledException(context, exception);
                break;
        }
    }

    private async Task HandleInfrastructureException(HttpContext context, InfrastructureException infrastructureException)
    {
        var errors = new List<ExceptionDetails>
        {
            ExceptionDetails.Create(Exceptions.Service_Is_Not_Available, infrastructureException.ErrorCode,
                infrastructureException.GetType().ToString())
        };

        await WriteExceptionToResponse(context, errors);
    }

    private async Task HandleBusinessException(HttpContext context, BusinessException businessException)
    {
        var errors = new List<ExceptionDetails>
        {
            ExceptionDetails.CreateBusinessException(businessException.ExceptionMessage, businessException.ErrorCode,
                businessException.GetType().ToString())
        };

        await WriteExceptionToResponse(context, errors);
    }

    private async Task WriteExceptionToResponse(HttpContext context, List<ExceptionDetails> errors)
    {
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        context.Response.ContentType = "application/json";

        var jsonSerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        var errorModel = new ErrorModel
        {
            Errors = errors
        };

        await context.Response.WriteAsync(JsonConvert.SerializeObject(errorModel, jsonSerializerSettings));
    }

    private async Task UnhandledException(HttpContext httpContext, Exception exception)
    {
        var errors = new List<ExceptionDetails>
        {
            ExceptionDetails.Create(Exceptions.There_Was_A_Problem_With_The_Request, -1000,
                string.Empty)
        };

        await WriteExceptionToResponse(httpContext, errors);
    }

    private bool IsUnhandledExceptionsCapturedBySentry()
    {
        return _exceptionLogConfiguration.CaptureBySentry;
    }

    private void CaptureOnSentry(Exception exception)
    {
        using (SentrySdk.Init(option =>
               {
                   option.Environment = _exceptionLogConfiguration.SentryConfiguration.Environment;
                   option.Dsn = _exceptionLogConfiguration.SentryConfiguration.Dsn;
               }))
        {
            SentrySdk.CaptureException(exception);
        }
    }
}

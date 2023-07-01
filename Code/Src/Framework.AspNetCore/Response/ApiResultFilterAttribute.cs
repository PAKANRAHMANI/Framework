using System;
using System.Collections.Generic;
using System.Linq;
using Framework.AspNetCore.MiddleWares;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Framework.AspNetCore.Response;

public class ApiResultFilterAttribute : ActionFilterAttribute
{
    public override void OnResultExecuting(ResultExecutingContext context)
    {
        if (context.Result is ObjectResult objectResult)
        {
            if (objectResult.StatusCode.HasValue)
            {
                if (objectResult.StatusCode.Value >= 200 && objectResult.StatusCode.Value < 300)
                {
                    int status = objectResult.StatusCode.Value;

                    if (status == StatusCodes.Status200OK || status == StatusCodes.Status201Created)
                    {

                        objectResult.Value = objectResult.Value != null ? ResponseModel.FromData(objectResult.Value) : null;
                    }
                    else if (status == StatusCodes.Status204NoContent)
                    {
                        objectResult.Value = null;
                    }
                    else
                    {
                        objectResult.Value = objectResult.Value != null ? ResponseModel.FromData(objectResult.Value) : null;
                    }
                }
                else if (objectResult.StatusCode.Value >= 400 && objectResult.StatusCode.Value < 600)
                {
                    if (objectResult.Value is IEnumerable<ExceptionDetails> errorModels)
                    {
                        objectResult.Value = ResponseModel.FromError(errorModels.ToList());
                    }
                    else
                    {
                        throw new Exception("Invalid error model.");
                    }
                }
            }
        }

        base.OnResultExecuting(context);
    }
}
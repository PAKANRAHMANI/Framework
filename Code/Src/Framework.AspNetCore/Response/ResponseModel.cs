using Framework.AspNetCore.MiddleWares;
using System.Collections.Generic;

namespace Framework.AspNetCore.Response;

public abstract class ResponseModel
{
    public static DataResponseModel<TData> FromData<TData>(TData data) => new DataResponseModel<TData> { Data = data };
    public static ErrorResponseModel FromError(List<ExceptionDetails> errors) => new ErrorResponseModel { Errors = errors };
}
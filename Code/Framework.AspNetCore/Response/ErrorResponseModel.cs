using Framework.AspNetCore.MiddleWares;
using System.Collections.Generic;

namespace Framework.AspNetCore.Response;

public class ErrorResponseModel : ResponseModel
{
    public List<ExceptionDetails> Errors { get; set; }
}
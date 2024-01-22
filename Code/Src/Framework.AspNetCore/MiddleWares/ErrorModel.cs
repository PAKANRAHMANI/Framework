using System.Collections.Generic;

namespace Framework.AspNetCore.MiddleWares;

public class ErrorModel
{
    public List<ExceptionDetails> Errors { get; set; }
}
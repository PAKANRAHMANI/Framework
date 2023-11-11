using System;

namespace Framework.Core.Exceptions;

public class InfrastructureException : Exception
{
    public long ErrorCode { get; private set; }
    public string ExceptionMessage { get; private set; }
    protected InfrastructureException() { }
    public InfrastructureException(long errorCode, string message)
    {
        this.ErrorCode = errorCode;
        this.ExceptionMessage = message;
    }
}
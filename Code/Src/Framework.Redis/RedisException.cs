using System;

namespace Framework.Redis;

public class RedisException : Exception
{
    public string ExceptionMessage { get; private set; }
    public string MethodName { get; private set; }
    protected RedisException() { }

    public RedisException(string exceptionMessage, string methodName)
    {
        this.ExceptionMessage = exceptionMessage;
        this.MethodName = methodName;
    }
}
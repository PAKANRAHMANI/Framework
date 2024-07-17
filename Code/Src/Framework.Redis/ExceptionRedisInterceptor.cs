using System;
using Castle.DynamicProxy;

namespace Framework.Redis;

public class ExceptionRedisInterceptor : IInterceptor
{
    public void Intercept(IInvocation invocation)
    {
        try
        {
            invocation.Proceed();
        }
        catch (Exception ex)
        {

            throw new RedisException(ex.Message, invocation.Method.Name);
        }
    }
}
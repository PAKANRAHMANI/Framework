using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Core.Logging
{
    public interface ILogger
    {
        void Write(string message, LogLevel level);
        void Write(string template, LogLevel level, params object[] parameters);
        void WriteException(Exception exception);
    }
}

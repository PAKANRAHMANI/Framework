using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Core.Exceptions
{
    public class FrameworkException: Exception
    {
        public long Code { get; private set; }
        public string ExceptionMessage { get; private set; }
        protected FrameworkException() { }
        public FrameworkException(long code, string message)
        {
            this.Code = code;
            this.ExceptionMessage = message;
        }
        public FrameworkException(Enum errorCode, string errorMessage)
        {
            this.Code = Convert.ToInt32(errorCode);
            this.ExceptionMessage = errorMessage;
        }
    }
}

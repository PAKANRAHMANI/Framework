using Framework.Core.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Core.Exceptions
{
    public class BusinessException : Exception
    {
        #region Properties

        public long ErrorCode { get; private set; }
        public string ExceptionMessage { get; private set; }
        public long BoundedContextCode { get; private set; } 

        #endregion

        #region Constructors

        protected BusinessException() { }
        public BusinessException(long code,
            string message,
            long boundedContextCode = BoundedContextCodes.Default)
        {
            this.ErrorCode = code;
            this.ExceptionMessage = message;
            this.BoundedContextCode = boundedContextCode;
        }

        public BusinessException(Enum errorCode,
            string errorMessage,
            long boundedContextCode = BoundedContextCodes.Default)
        {
            this.ErrorCode = Convert.ToInt32(errorCode);
            this.ExceptionMessage = errorMessage;
            this.BoundedContextCode = boundedContextCode;
        }

        #endregion
    }
}

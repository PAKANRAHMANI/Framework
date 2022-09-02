using System;

namespace Framework.Core.Exceptions
{
    public class BusinessException : Exception
    {
        private const long BoundedContextCodesDefault = -1000;
        public long ErrorCode { get; private set; }
        public string ExceptionMessage { get; private set; }
        public long BoundedContextCode { get; private set; }
        protected BusinessException() { }

        public BusinessException(string message)
        {
            this.ErrorCode = -1;
            this.ExceptionMessage = message;
            this.BoundedContextCode = BoundedContextCodesDefault;
        }
        public BusinessException(long errorCode, string message)
        {
            this.ErrorCode = errorCode;
            this.ExceptionMessage = message;
            this.BoundedContextCode = BoundedContextCodesDefault;
        }
        public BusinessException(Enum errorCode, string errorMessage)
        {
            this.ErrorCode = Convert.ToInt32(errorCode);
            this.ExceptionMessage = errorMessage;
            this.BoundedContextCode = BoundedContextCodesDefault;
        }
        public BusinessException(Enum errorCode, string errorMessage, long boundedContextCode)
        {
            this.ErrorCode = Convert.ToInt32(errorCode);
            this.ExceptionMessage = errorMessage;
            this.BoundedContextCode = boundedContextCode;
        }
        public BusinessException(long errorCode, string errorMessage, long boundedContextCode)
        {
            this.ErrorCode = errorCode;
            this.ExceptionMessage = errorMessage;
            this.BoundedContextCode = boundedContextCode;
        }
    }
}

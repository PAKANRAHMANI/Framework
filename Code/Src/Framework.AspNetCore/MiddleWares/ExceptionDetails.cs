using Framework.Core.Exceptions;

namespace Framework.AspNetCore.MiddleWares
{
    public class ExceptionDetails
    {
        public string Message { get; set; }
        public long Code { get; set; }
        public string Type { get; set; }

        private ExceptionDetails(string message, long code, string type)
        {
            this.Message = message;
            this.Code = code;
            this.Type = type;
        }

        public static ExceptionDetails Create(string message, long code, string type)
        {
            return new ExceptionDetails(message, code, "");
        }

        public static ExceptionDetails CreateBusinessException(string message, long code, string type)
        {
            if (type == typeof(EntityNotFoundException<>).FullName)
                message = Exceptions.Entity_Not_Found;

            return new ExceptionDetails(message, code, "");
        }
    }
}

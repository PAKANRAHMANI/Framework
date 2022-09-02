namespace Framework.AspNetCore.MiddleWares
{
    public class ExceptionDetails
    {
        public string Message { get; set; }
        public long Code { get; set; }

        private ExceptionDetails(string message, long code)
        {
            this.Message = message;
            this.Code = code;
        }

        public static ExceptionDetails Create(string message, long code)
        {
            return new ExceptionDetails(message,code);
        }
    }
}

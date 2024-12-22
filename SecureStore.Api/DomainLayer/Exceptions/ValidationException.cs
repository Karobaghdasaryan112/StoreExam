namespace SecureStore.Api.DomainLayer.Exceptions
{
    public class ValidateException : Exception
    {
        public string Msg { get; set; }
        public Exception InnarException { get; set; }
        public ValidateException(string msg) : base(msg)
        {
            Msg = msg;
        }
        public ValidateException(string msg, Exception innarExceptrion) : base(msg, innarExceptrion)
        {
            Msg = msg;
            InnarException = innarExceptrion;
        }

    }
}

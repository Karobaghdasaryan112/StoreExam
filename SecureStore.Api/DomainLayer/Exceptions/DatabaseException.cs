namespace SecureStore.Api.DomainLayer.Exceptions
{
    public class DatabaseException : Exception
    {
        public string Msg { get; set; }
        public Exception InnarException { get; set; }
        public DatabaseException(string msg) : base(msg)
        {
            Msg = msg;
        }
        public DatabaseException(string msg, Exception innarExceptrion) : base(msg, innarExceptrion)
        {
            Msg = msg;
            InnarException = innarExceptrion;
        }
    }
}

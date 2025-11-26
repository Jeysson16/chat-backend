namespace ChatModularMicroservice.Exceptions
{
    public class BaseException : System.Exception
    {
        public BaseException() { }
        public BaseException(string message) : base(message) { }
        public BaseException(string message, System.Exception inner) : base(message, inner) { }
    }
}
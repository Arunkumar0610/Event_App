namespace UserAuthenticatonService.Exceptions
{
    public class InvalidUserCredentialsException:ApplicationException
    {
        public InvalidUserCredentialsException() { }
        public InvalidUserCredentialsException(string message):base(message) { }
    }
}

namespace WebApiSimplyFly.Exceptions
{
    public class InvalidUserException:Exception
    {
        private readonly string message;
        public InvalidUserException()
        {
            message = "Invalid username or password";
        }
        public override string Message => message;

    }
}

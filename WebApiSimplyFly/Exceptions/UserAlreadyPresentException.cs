namespace WebApiSimplyFly.Exceptions
{
    public class UserAlreadyPresentException:Exception
    {
        private readonly string message;
        public UserAlreadyPresentException()
        {
            message = "A user already Exists";
        }
        public override string Message => message;

    }
}

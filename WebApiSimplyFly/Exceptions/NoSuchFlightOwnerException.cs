namespace WebApiSimplyFly.Exceptions
{
    public class NoSuchFlightOwnerException:Exception
    {
        private readonly string message;
        public NoSuchFlightOwnerException()
        {
            message = "No user with given username";
        }
        public override string Message => message;

    }
}

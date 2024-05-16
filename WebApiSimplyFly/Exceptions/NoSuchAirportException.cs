namespace WebApiSimplyFly.Exceptions
{
    public class NoSuchAirportException:Exception
    {
        private readonly string message;
        public NoSuchAirportException()
        {
            message = "No user with given username";
        }
        public override string Message => message;

    }
}

namespace WebApiSimplyFly.Exceptions
{
    public class NoSuchAirportException:Exception
    {
        private readonly string message;
        public NoSuchAirportException()
        {
            message = "No Airport with given ID";
        }
        public override string Message => message;

    }
}

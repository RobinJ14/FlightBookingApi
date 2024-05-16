namespace WebApiSimplyFly.Exceptions
{
    public class NoFlightAvailableException:Exception
    {
        private readonly string message;
        public NoFlightAvailableException()
        {
            message = "No Flight is available";
        }
        public override string Message => message;
    }
}

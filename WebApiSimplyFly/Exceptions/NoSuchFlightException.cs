namespace WebApiSimplyFly.Exceptions
{
    public class NoSuchFlightException:Exception
    {
        private readonly string message;
        public NoSuchFlightException()
        {
            message = "No Flight with the given Flight No";
        }
        public override string Message => message;

    }
}

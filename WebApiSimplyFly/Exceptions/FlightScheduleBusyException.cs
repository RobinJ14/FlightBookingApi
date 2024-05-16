namespace WebApiSimplyFly.Exceptions
{
    public class FlightScheduleBusyException:Exception
    {
        private readonly string message;
        public FlightScheduleBusyException()
        {
            message = "Flight is Busy ";
        }
        public override string Message => message;
    }
}

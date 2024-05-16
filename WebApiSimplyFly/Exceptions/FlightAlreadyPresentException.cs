namespace WebApiSimplyFly.Exceptions
{
    public class FlightAlreadyPresentException:Exception
    {
        private readonly string message;
        public FlightAlreadyPresentException()
        {
            message = "Flight Already Exists with same ID";
        }
        public override string Message => message;
    }
}

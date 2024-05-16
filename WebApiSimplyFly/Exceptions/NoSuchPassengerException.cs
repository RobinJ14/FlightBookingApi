namespace WebApiSimplyFly.Exceptions
{
    public class NoSuchPassengerException:Exception
    {
        private readonly string message;
        public NoSuchPassengerException()
        {
            message = "No Passenger with given id";
        }
        public override string Message => message;

    }
}

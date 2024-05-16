namespace WebApiSimplyFly.Exceptions
{
    public class NoSuchPassengerBookingException:Exception
    {

        private readonly string message;
        public NoSuchPassengerBookingException()
        {
            message = "No Passenger Booking with given Id";
        }
        public override string Message => message;
    }
}

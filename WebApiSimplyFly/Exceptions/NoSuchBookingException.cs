namespace WebApiSimplyFly.Exceptions
{
    public class NoSuchBookingException:Exception
    {

        private readonly string message;
        public NoSuchBookingException()
        {
            message = "No Booking with given Id";
        }
        public override string Message => message;
    }
}

namespace WebApiSimplyFly.Exceptions
{
    public class AirportAlreadyPresentException:Exception
    {
        private readonly string message;
        public AirportAlreadyPresentException()
        {
            message = "An Airport already Exists";
        }
        public override string Message => message;
    }
}

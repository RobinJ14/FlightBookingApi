namespace WebApiSimplyFly.Exceptions
{
    public class NoSuchPaymentException:Exception
    {
        private readonly string message;
        public NoSuchPaymentException()
        {
            message = "No Payment with the given id";
        }
        public override string Message => message;

    }
}

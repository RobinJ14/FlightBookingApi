namespace WebApiSimplyFly.Exceptions
{
    public class NoSuchPaymentDetailsException:Exception
    {
        private readonly string message;
        public NoSuchPaymentDetailsException()
        {
            message = "No Details with the given ID exists";
        }
        public override string Message => message;

    }
}

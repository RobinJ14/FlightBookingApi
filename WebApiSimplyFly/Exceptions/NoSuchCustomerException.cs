namespace WebApiSimplyFly.Exceptions
{
    public class NoSuchCustomerException:Exception
    {
        private readonly string message;
        public NoSuchCustomerException()
        {
            message = "No Customer with given CustomerId";
        }
        public override string Message => message;
    }
}

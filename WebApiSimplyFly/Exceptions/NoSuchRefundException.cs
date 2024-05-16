namespace WebApiSimplyFly.Exceptions
{
    public class NoSuchRefundException : Exception
    {

        private readonly string message;
        public NoSuchRefundException()
        {
            message = "No Refund with given Id";
        }
        public override string Message => message;
    }
}

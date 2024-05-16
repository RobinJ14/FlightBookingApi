namespace WebApiSimplyFly.Exceptions
{
    public class NoSuchHistoryException:Exception
    {
        private readonly string message;
        public NoSuchHistoryException()
        {
            message = "No History with given Id";
        }
        public override string Message => message;
    }
}

namespace WebApiSimplyFly.Exceptions
{
    public class NoSuchScheduleException : Exception
    {
        private readonly string message;
        public NoSuchScheduleException()
        {
            message = "No seat with the given ID exists";
        }
        public override string Message => message;
    }
}

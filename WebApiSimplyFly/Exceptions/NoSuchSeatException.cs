namespace WebApiSimplyFly.Exceptions
{
    public class NoSuchSeatException:Exception
    {

        private readonly string message;
        public NoSuchSeatException()
        {
            message = "No seat with the given ID exists";
        }
        public override string Message => message;
    }
}

namespace WebApiSimplyFly.Exceptions
{
    public class NoSuchRouteException:Exception
    {

        private readonly string message;
        public NoSuchRouteException()
        {
            message = "No Route with given ID";
        }
        public override string Message => message;
    }
}

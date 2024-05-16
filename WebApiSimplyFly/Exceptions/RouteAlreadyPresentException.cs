namespace WebApiSimplyFly.Exceptions
{
    public class RouteAlreadyPresentException: Exception
    {
        private readonly string message;
        public RouteAlreadyPresentException()
        {
            message = "A Route already Exists";
        }
        public override string Message => message;
    }
}

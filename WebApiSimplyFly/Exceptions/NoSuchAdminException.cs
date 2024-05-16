namespace WebApiSimplyFly.Exceptions
{
    public class NoSuchAdminException:Exception
    {
        private readonly string message;
        public NoSuchAdminException()
        {
            message = "No user with given username";
        }
        public override string Message => message;

    }
}

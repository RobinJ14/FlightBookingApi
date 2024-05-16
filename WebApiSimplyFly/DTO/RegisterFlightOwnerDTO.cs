namespace WebApiSimplyFly.DTO
{
    public class RegisterFlightOwnerDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } = "flightowner";

        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string BusinessRegistrationNumber { get; set; } = string.Empty;


        
    }
}

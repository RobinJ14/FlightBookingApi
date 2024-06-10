namespace WebApiSimplyFly.DTO
{
    public class RegisterCustomerDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } = "customer";

        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string? Phone { get; set; }

        public string? Gender { get; set; }
    }
}

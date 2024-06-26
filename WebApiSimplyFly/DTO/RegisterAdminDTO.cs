﻿namespace WebApiSimplyFly.DTO
{
    public class RegisterAdminDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } = "admin";
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? Address { get; set; }
    }
}

﻿namespace WebApiSimplyFly.DTO
{
    public class UpdateCustomerDTO
    {
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string Username { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiSimplyFly.Models
{
    public class Admin
    {
        [Key]
        public int AdminId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string Username { get; set; }
        [ForeignKey("Username")]
        public User User { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace WebApiSimplyFly.Models
{
    [ExcludeFromCodeCoverage]
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Address { get; set; }

        public string? Phone { get; set; }
        public string? Gender { get; set; }
        public string username { get; set; }
        [ForeignKey("username")]
        public User User { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace WebApiSimplyFly.Models
{
    public class Passenger
    {
        [Key]
        public int PassengerId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; } = 0;
        public string PassportNo { get; set; } = string.Empty;
    }
}

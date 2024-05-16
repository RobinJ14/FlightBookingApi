using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiSimplyFly.Models
{
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }
        public double Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public PaymentStatus Status { get; set; }

       
        public PaymentDetails PaymentDetails { get; set; } = new PaymentDetails();

        public enum PaymentStatus
        {
            Successful,
            Failed,
            RefundIssued
        }

       

    }
    public class PaymentDetails
    {
        public int PaymentDetailsId { get; set; }

        public string CardNumber { get; set; } = string.Empty;
        public string ExpiryDate { get; set; } = string.Empty;
        public string CVV { get; set; } = string.Empty;

    }
}

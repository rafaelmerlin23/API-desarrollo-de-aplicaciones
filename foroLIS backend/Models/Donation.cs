using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace foroLIS_backend.Models
{
    public class Donation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        
        public string DonorId { get; set; }
        [ForeignKey("DonorId")]
        public virtual Users Donor { get; set; }
        public string ReceiverId { get; set; }
        [ForeignKey("ReceiverId")]
        public virtual Users Receiver { get; set; }
        public Guid PostId { get; set; }
        [ForeignKey("PostId")]
        public virtual Post Post { get; set; }
        [Precision(18, 2)]
        public Decimal Amount { get; set; }

        public string PaymentId { get; set; }
        public string Status { get; set; }
        public string StatusDetail { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public DateTime? DateApproved { get; set; }

        public string Description { get; set; }
        public string Currency { get; set; } = "ARS";
    }

}

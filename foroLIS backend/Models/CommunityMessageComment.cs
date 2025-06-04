using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace foroLIS_backend.Models
{
    public class CommunityMessageComment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public string Title { get; set; }

        public DateTime CreateAt { get; set; } = DateTime.UtcNow;

        public Guid CommunityMessageId { get; set; }

        [ForeignKey("CommunityMessageId")]
        public virtual CommunityMessage CommunityMessage { get; set; }

        [Required]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual Users User { get; set; }
    }
}

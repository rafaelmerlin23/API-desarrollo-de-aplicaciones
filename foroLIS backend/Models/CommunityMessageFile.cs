using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace foroLIS_backend.Models
{
    public class CommunityMessageFile
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Guid CommunityMessageId { get; set; }
        public Guid FileId { get; set; }
        [ForeignKey("FileId")]
        public virtual MediaFile MediaFile { get; set; }
        [ForeignKey("CommunityMessageId")]
        public virtual CommunityMessage CommunityMessage { get; set; }
    }
}

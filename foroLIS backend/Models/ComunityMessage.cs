using foroLIS_backend.DTOs.CommunityMessagesDto;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace foroLIS_backend.Models
{
    public class CommunityMessage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Texto { get; set; }
        public DateTime Fecha { get; set; }
        public Guid PostId { get; set; }
        [ForeignKey("PostId")]
        public virtual Post Post { get; set; }

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual Users User { get; set; }

        public virtual CommunitySurvey? Survey { get; set; }
        public CommunityMessageDto ToDto()
        {
            return new CommunityMessageDto
            {
                Fecha = this.Fecha,
                Id = this.Id,
                Texto = this.Texto,
                UserId = this.UserId,
            };
        }
    }

}

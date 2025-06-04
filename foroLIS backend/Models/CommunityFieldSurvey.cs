using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace foroLIS_backend.Models
{
    public class CommunityFieldSurvey
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Title {  get; set; }
        public Guid SurveyId { get; set; }
        [ForeignKey("SurveyId")]
        public virtual CommunitySurvey Survey { get; set; }
        public ICollection<CommunityFieldsUser> Users { get; set; } = new List<CommunityFieldsUser>();


    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace foroLIS_backend.Models
{
    public class CommunitySurvey
    {

        [Key, ForeignKey("CommunityMessage")]
        public Guid Id { get; set; }

        public required string Title { get; set; }

        public virtual CommunityMessage CommunityMessage { get; set; }
        public bool AllowMoreOneAnswer { get; set; }
        public DateTime EndDate { get; set; }

        public ICollection<CommunityFieldSurvey> Fields { get; set; } = new List<CommunityFieldSurvey>();
    }
}

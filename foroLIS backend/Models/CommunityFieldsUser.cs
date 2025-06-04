using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace foroLIS_backend.Models
{
    public class CommunityFieldsUser
    {
        public Guid CommunityFieldId { get; set; }
        public virtual CommunityFieldSurvey CommunityFieldSurvey { get; set; }

        public string UserId { get; set; }
        public virtual Users User { get; set; }
    }
}

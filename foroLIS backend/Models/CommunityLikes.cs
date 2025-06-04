namespace foroLIS_backend.Models
{
    public class CommunityLikes
    {
        public Guid CommunityMessageId { get; set; }
        public virtual CommunityMessage CommunityMessage { get; set; }
        public string UserId { get; set; }
        public virtual Users User { get; set; }
    }
}

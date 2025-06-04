namespace foroLIS_backend.DTOs.CommunityMessagesDto
{
    public class UpdateLikeDto
    {
        public Guid CommunityMessageId { get; set; }
        public string UserId { get; set; }
        public string option { get; set; } = "add"; // add o remove 
    }

    public class LikeUnlikeDto
    {
        public Guid CommunityMessageId { get; set; }
    }

}

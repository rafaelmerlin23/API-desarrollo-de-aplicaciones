namespace foroLIS_backend.DTOs.CommunityMessagesCommentsDtos;

public class CommunityMessageCommentDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public DateTime CreateAt { get; set; }
    public Guid CommunityMessageId { get; set; }
    public string UserId { get; set; }
}
namespace foroLIS_backend.DTOs.CommunityMessagesCommentsDtos;

public class CommunityMessageCommentCreateDto
{
    public string Title { get; set; }
    public Guid CommunityMessageId { get; set; }
    public string UserId { get; set; }
}
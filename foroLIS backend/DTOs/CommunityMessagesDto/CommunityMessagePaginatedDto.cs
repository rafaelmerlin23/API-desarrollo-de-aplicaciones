namespace foroLIS_backend.DTOs.CommunityMessagesDto
{
    public class CommunityMessagePaginatedDto
    {
        public Guid postId { get; set; }
        public int page { get; set; } = 1;
        public int pageSize { get; set; } = 10;
    }
}

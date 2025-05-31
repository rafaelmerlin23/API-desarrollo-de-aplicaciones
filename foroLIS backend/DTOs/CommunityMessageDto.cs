namespace foroLIS_backend.DTOs
{
    public class CommunityMessageDto
    {
        public Guid Id { get; set; }
        public string Texto { get; set; }
        public DateTime Fecha { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
    }

}

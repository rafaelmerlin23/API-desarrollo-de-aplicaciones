namespace foroLIS_backend.DTOs.CommunityMessagesDto
{
    public class CommunityMessageDto
    {
        public Guid Id { get; set; }
        public string Texto { get; set; }
        public DateTime Fecha { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; } 
        public bool isOwner { get; set; }
    }

}

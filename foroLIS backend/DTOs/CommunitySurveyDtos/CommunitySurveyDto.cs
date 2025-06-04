namespace foroLIS_backend.DTOs.CommunitySurveyDtos
{
    public class CommunitySurveyDto
    {
        public Guid CommunityMessageId { get; set; }
        public string Title { get; set; }
        public List<CommunityFieldDto> Fields { get; set; }
        public bool AllowMoreOneAnswer { get; set; }
        public DateTime? EndDate { get; set; }

    }
}

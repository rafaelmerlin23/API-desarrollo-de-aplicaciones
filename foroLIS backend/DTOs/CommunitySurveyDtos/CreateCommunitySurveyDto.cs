namespace foroLIS_backend.DTOs.CommunitySurveyDtos
{
    public class CreateCommunitySurveyDto
    {
        public string Title { get; set; }
        public List<CreateCommunityFieldDto> Fields { get; set; }
        public bool AllowMoreOneAnswer { get; set; }
        public DateTime EndDate { get; set; } = DateTime.UtcNow.AddDays(1);

    }

    public class CreateCommunitySurvey
    {
        public Guid CommunityMessageId { get; set; }
        public string Title { get; set; }
        public List<CreateCommunityFieldDto> Fields { get; set; }
        public bool AllowMoreOneAnswer { get; set; }
        public DateTime? EndDate { get; set; }

    }
}

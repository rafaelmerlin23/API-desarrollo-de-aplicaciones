namespace foroLIS_backend.DTOs.CommunitySurveyDtos
{
    public class CommunityFieldDto
    {
        public Guid Id { get; set; }
        public string Title {  get; set; }
        public Guid SurveyId {  get; set; }
        public int? votes { get; set; }
        public bool isvote { get; set; }

    }
}

using foroLIS_backend.DTOs.CommunitySurveyDtos;

namespace foroLIS_backend.DTOs.CommunityMessagesDto
{
    public class CreateCommunityMessagesDto
    {
        public string Texto { get; set; }
        public Guid PostId { get; set; }
        public CreateCommunitySurveyDto? survey {get; set;}
    }

    public class CreateCommunityMessages
    {
        public string Texto { get; set; }
        public Guid PostId { get; set; }
        public CreateCommunitySurvey? survey { get; set; }
    }
}

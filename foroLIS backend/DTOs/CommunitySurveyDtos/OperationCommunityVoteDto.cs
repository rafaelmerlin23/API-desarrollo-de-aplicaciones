namespace foroLIS_backend.DTOs.CommunitySurveyDtos
{
    public class OperationCommunityVoteDto
    {
        public Guid FieldId { get; set; }
        public string Operation { get; set; } = "add"; // valores posibles: "add", "remove"
    }

}

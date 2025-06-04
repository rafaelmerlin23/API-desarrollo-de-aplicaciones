using foroLIS_backend.DTOs.CommunitySurveyDtos;
using foroLIS_backend.DTOs.FileDto;

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
        public CommunitySurveyDto? Survey { get; set; }

        public int? likes { get; set; }
        public bool? isLiked { get; set; }
        public List<LinksFile> files { get; set; }
    }



}

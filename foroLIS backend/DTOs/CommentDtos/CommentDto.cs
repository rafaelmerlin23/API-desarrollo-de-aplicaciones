namespace foroLIS_backend.DTOs.CommentDtos;

public class CommentDto
{
    public Guid Id { get; set; }
    public string ArchitectureOS { get; set; }
    public string FamilyOS { get; set; }
    public string VersionOS { get; set; }
    public Guid PostId { get; set; }
    public string UserId { get; set; }
    public Guid? MediaFileId { get; set; }
    public DateTime CreateAt { get; set; }
    public DateTime UpdateAt { get; set; }
}


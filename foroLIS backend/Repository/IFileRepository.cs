using foroLIS_backend.DTOs.FileDto;
using foroLIS_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace foroLIS_backend.Repository
{
    public interface IFileRepository<TEntity>
    {
        public Task Create(TEntity file);

        public Task Save();

        public Task<AddCommunityMessageFileDto> AddFileToCommunityMessage(CommunityMessageFile cmf);

        Task<List<CommunityMessageFile>> GetCommunityMessageFilesByMessageId(Guid messageId);
        Task DeleteCommunityMessageFile(CommunityMessageFile relation);
        Task DeleteMediaFile(MediaFile file);
        public Task<AddPostFileDto> AddFileToPost(FilePost cmf);

    }
}

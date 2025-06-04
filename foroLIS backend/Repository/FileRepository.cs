using foroLIS_backend.DTOs.FileDto;
using foroLIS_backend.Infrastructure.Context;
using foroLIS_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace foroLIS_backend.Repository
{
    public class FileRepository: IFileRepository<MediaFile>
    {
        private readonly ApplicationDbContext _context;
        public FileRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Create( MediaFile file)
        {   
            await _context.MediaFiles.AddAsync(file);
        }

        public async Task<AddCommunityMessageFileDto> AddFileToCommunityMessage(CommunityMessageFile cmf)
        {
            await _context.CommunityMessageFiles.AddAsync(cmf);
            await _context.SaveChangesAsync();
            return new AddCommunityMessageFileDto
            {
                CommunityMessageId = cmf.CommunityMessageId,
                MediaFileId = cmf.FileId
            };
        }

        public async Task<List<CommunityMessageFile>> GetCommunityMessageFilesByMessageId(Guid messageId)
        {
            return await _context.CommunityMessageFiles
                .Include(cmf => cmf.MediaFile)
                .Where(cmf => cmf.CommunityMessageId == messageId)
                .ToListAsync();
        }

        public async Task DeleteCommunityMessageFile(CommunityMessageFile relation)
        {
            _context.CommunityMessageFiles.Remove(relation);
            await Task.CompletedTask;
        }

        public async Task DeleteMediaFile(MediaFile file)
        {
            _context.MediaFiles.Remove(file);
            await Task.CompletedTask;
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}

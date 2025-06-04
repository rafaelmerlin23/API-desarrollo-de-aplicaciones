using foroLIS_backend.DTOs.FileDto;
using foroLIS_backend.Models;
using foroLIS_backend.Repository;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;


namespace foroLIS_backend.Services
{
    public class FileService
    {
        private readonly string _route = Path.Combine(Directory.GetCurrentDirectory(), "FilesUploaded");
        
        private readonly string[] _extensions_shorts = [".png", ".jpg", ".webp",".jpeg"]; 
        
        private readonly IHttpContextAccessor _httpContextAccessor;
        
        private readonly IFileRepository<MediaFile> _fileRepository;
        
        private readonly string folderFilesName = "files";

        public FileService(IFileRepository<MediaFile> fileRepository,
            IHttpContextAccessor httpContextAccessor) 
        {
            _fileRepository = fileRepository;   
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task DeleteFilesByCommunityMessageId(Guid communityMessageId)
        {
            var relations = await _fileRepository.GetCommunityMessageFilesByMessageId(communityMessageId);

            foreach (var relation in relations)
            {
                var file = relation.MediaFile;
                if (file == null) continue;

                // Eliminar archivo original del disco
                if (File.Exists(file.FilePath))
                {
                    File.Delete(file.FilePath);
                }

                // Eliminar archivo reducido (miniatura) si es imagen
                var ext = Path.GetExtension(file.FilePath).ToLower();
                var imageExts = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
                if (imageExts.Contains(ext))
                {
                    var shortPath = Path.Combine(_route, "short_" + file.FileName);
                    if (File.Exists(shortPath))
                    {
                        File.Delete(shortPath);
                    }
                }

                // Eliminar relación y archivo de la base de datos
                await _fileRepository.DeleteCommunityMessageFile(relation);
                await _fileRepository.DeleteMediaFile(file);
            }

            await _fileRepository.Save();
        }


        public async Task<AddCommunityMessageFileDto> AddFileToCommunityMessage(AddCommunityMessageFileDto request)
        {
            var newFileCommunityMessage = new CommunityMessageFile
            {
                CommunityMessageId = request.CommunityMessageId,
                FileId = request.MediaFileId
            };
            await _fileRepository.AddFileToCommunityMessage(newFileCommunityMessage);
            return new AddCommunityMessageFileDto
            {
                MediaFileId = request.MediaFileId,
                CommunityMessageId = request.CommunityMessageId,
            };
        }

        public async Task<AddPostFileDto> AddFileToPost(AddPostFileDto request)
        {
            var newFileCommunityMessage = new FilePost
            {
               FileId = request.FileId,
                PostId= request.PostId,
            };
            await _fileRepository.AddFileToPost(newFileCommunityMessage);
            
            return request;
        }

        public async Task<FileUploadDto> UploadFile(IFormFile file)
        {
            if (!Directory.Exists(_route))
            {
                Directory.CreateDirectory(_route);
            }

            var name = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var route = Path.Combine(_route, name);

            using (var stream = new FileStream(route, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            bool isImage = _extensions_shorts.Contains(Path.GetExtension(file.FileName.ToLower()));
            
            string shortPath = "";

            if (isImage)
            {
                var shortFileName = "short_" + name;
                shortPath = Path.Combine(_route, shortFileName);

                using (var image = await Image.LoadAsync(route)) {
                    
                    image.Mutate(x => x.Resize(new ResizeOptions
                    {
                        Size = new Size(500, 500), 
                        Mode = ResizeMode.Max      
                    }));

                    await image.SaveAsync(shortPath);
                }
            }
            var newFile = new MediaFile()
            {
                CreateAt = DateTime.Now,
                FileName = name,
                FilePath = route,
            };

            await _fileRepository.Create(newFile);
            await _fileRepository.Save();
            
            var baseUrl = _httpContextAccessor.HttpContext?.Request.Host.ToString();

            return new FileUploadDto()
            { 
                Id = newFile.Id,
                Name = name,
                Link = new LinksFile
                {
                    Original = $"{baseUrl}/{folderFilesName}/{name}",
                    Short = isImage ? $"{baseUrl}/{folderFilesName}/short_{name}" : null
                }
            };
        }
        
    }
}
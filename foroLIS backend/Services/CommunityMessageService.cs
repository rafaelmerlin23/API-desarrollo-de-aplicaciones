using foroLIS_backend.DTOs;
using foroLIS_backend.DTOs.CommunityMessagesCommentsDtos;
using foroLIS_backend.DTOs.CommunityMessagesDto;
using foroLIS_backend.DTOs.CommunitySurveyDtos;
using foroLIS_backend.DTOs.FileDto;
using foroLIS_backend.DTOs.PostDtos;
using foroLIS_backend.Infrastructure.Context;
using foroLIS_backend.Models;
using foroLIS_backend.Repository;
using MercadoPago.Resource.User;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace foroLIS_backend.Services
{
    public class CommunityMessageService: ICommunityMessageService<
        CommunityMessageDto,
        CreateCommunityMessagesDto,
        UpdateCommunityMessageDto,
        DeleteCommunityMessageDto>
    {
        private readonly ICommunityMessagesRepository<CommunityMessage,CommunityMessageDto> _repository;
        private readonly IUserService _userService;
        IRepository<Post, PostDto> _postRepository;
        private readonly ApplicationDbContext _context;
        private readonly ICommunitySurveyRepository _surveyRepository;
        private readonly FileService _fileService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CommunityMessageService(
        ICommunityMessagesRepository<CommunityMessage, CommunityMessageDto> repository,
        IUserService userService,
        IRepository<Post, PostDto> postRepository,
        ICommunitySurveyRepository surveyRepository,
        ApplicationDbContext context,
        FileService fileService,
        IHttpContextAccessor httpContextAccessor) { 
            _repository = repository;
            _userService = userService;   
            _postRepository = postRepository;
            _surveyRepository = surveyRepository;
            _context = context;
            _fileService = fileService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<PaginatedResponse<CommunityMessageDto>> GetPaginatedAsync(Guid postId, int page = 1, int pageSize = 10)
        {
            var Cms = await _repository.GetPaginatedAsync(postId, page, pageSize);
            return Cms;
        }

        public async Task<CommunityMessageDto> Delete(DeleteCommunityMessageDto request)
        {
            await _fileService.DeleteFilesByCommunityMessageId(request.Id);
            var user = await _userService.GetCurrentUserAsync();
            var cm = await _repository.GetById(request.Id);
            var dto = cm.ToDto();
            var userName = cm.User.UserName;
            var isOwner = cm.Post.User.Id == user.Id;
            dto.UserName = userName ?? "NA";
            dto.isOwner = isOwner;
            
            _repository.Delete(cm);
            await _repository.Save();
            
            return dto;
        }

        public async Task<UpdateLikeDto> LikeUnlike(Guid CommunityMessageId)
        {
            var response = await _repository.LikeUnlike(CommunityMessageId);
            return response;
        }

        public async Task<CommunityMessageDto> GetById(Guid id)
        {
            var baseUrl = _httpContextAccessor.HttpContext?.Request.Host.ToString();
            var user = await _userService.GetCurrentUserAsync();

            var cm = await _context.CommunityMessages
                .Where(c => c.Id == id)
                .Include(c => c.Survey)
                    .ThenInclude(s => s.Fields)
                .Include(c => c.User)
                .Include(c => c.Post)
                    .ThenInclude(p => p.User)
                .FirstOrDefaultAsync();

            if (cm == null)
                return null;

            var dto = new CommunityMessageDto
            {
                Id = cm.Id,
                Texto = cm.Texto,
                Fecha = cm.Fecha,
                UserId = cm.UserId,
                comments = _context.CommunityMessageComments
                .Where(cmm => cmm.CommunityMessageId == cm.Id)
                .OrderBy(cmm => cmm.CreateAt)
                .Take(3)
                .Select(cmm => new CommunityMessageCommentDto
                {
                    CommunityMessageId = cm.Id,
                    CreateAt = cmm.CreateAt,
                    Id = cmm.Id,
                    Title = cmm.Title,
                    UserId = cmm.UserId
                })
                .ToList(),
                files = _context.CommunityMessageFiles
                     .Where(cf => cf.CommunityMessageId == cm.Id)
                     .Select(cf => new LinksFile
                     {
                         Original = $"{baseUrl}/files/{cf.MediaFile.FileName}",
                         Short = cf.MediaFile.FileName.ToLower().EndsWith(".jpg") || cf.MediaFile.FileName.ToLower().EndsWith(".png") || cf.MediaFile.FileName.ToLower().EndsWith(".jpeg")
                             ? $"{baseUrl}/files/short_{cf.MediaFile.FileName}"
                             : null
                     })
                     .ToList(),
                UserName = cm.User?.UserName ?? "NA",
                isOwner = cm.Post.User.Id == user.Id,
                likes = await _context.CommunityLikes.CountAsync(cl => cl.CommunityMessageId == cm.Id),
                isLiked = await _context.CommunityLikes.AnyAsync(cl => cl.CommunityMessageId == cm.Id && cl.UserId == user.Id),
                Survey = cm.Survey == null ? null : new CommunitySurveyDto
                {
                    CommunityMessageId = cm.Survey.Id,
                    Title = cm.Survey.Title,
                    AllowMoreOneAnswer = cm.Survey.AllowMoreOneAnswer,
                    EndDate = cm.Survey.EndDate,
                    Fields = cm.Survey.Fields.Select(f => new CommunityFieldDto
                    {
                        Id = f.Id,
                        Title = f.Title,
                        SurveyId = f.SurveyId,
                        votes = _context.CommunityUserFields.Count(cuf => cuf.CommunityFieldId == f.Id),
                        isvote = _context.CommunityUserFields.Any(cuf => cuf.CommunityFieldId == f.Id && cuf.UserId == user.Id)
                    }).ToList()
                }
            };

            return dto;
        }



        public async Task<CommunityMessageDto> Update(UpdateCommunityMessageDto request)
        {
            var user = await _userService.GetCurrentUserAsync();
            var cm = await _repository.GetById(request.Id);
            cm.Texto = request.Texto;
            _repository.Update(cm);
            await _repository.Save();
            var dto = cm.ToDto();
            var userName = cm.User.UserName;
            var isOwner = cm.Post.User.Id == user.Id;
            dto.UserName = userName ?? "NA";
            dto.isOwner = isOwner;
            return dto;
        }

        public async Task<CommunityMessageDto> Add(CreateCommunityMessagesDto request)
        {
            var user = await _userService.GetCurrentUserAsync();
            var post = await _postRepository.GetById(request.PostId);
            var cm = new CommunityMessage
            {
                Fecha = DateTime.UtcNow,
                PostId = request.PostId,
                UserId = user.Id,
                Texto = request.Texto,
            };

            await _repository.Add(cm);
            await _repository.Save();
            
            var dto = cm.ToDto();
            dto.UserName = user.FullName ?? "NA";
            dto.isOwner = post.User.Id == user.Id;
            if(request.survey != null)
            {
                var newsurvey = new CreateCommunitySurvey()
                {
                    AllowMoreOneAnswer = request.survey.AllowMoreOneAnswer,
                    EndDate = request.survey.EndDate,
                    Fields = request.survey.Fields,
                    Title = request.survey.Title,
                    CommunityMessageId = cm.Id,
                };

                var result = await _surveyRepository.CreateSurvey(newsurvey);
                dto.likes = 0;
                dto.Survey = result;
            }
            return dto;
        }
    }
}

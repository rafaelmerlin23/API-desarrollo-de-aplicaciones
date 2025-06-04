using foroLIS_backend.DTOs;
using foroLIS_backend.DTOs.CommunityMessagesCommentsDtos;
using foroLIS_backend.DTOs.CommunityMessagesDto;
using foroLIS_backend.DTOs.CommunitySurveyDtos;
using foroLIS_backend.DTOs.FileDto;
using foroLIS_backend.DTOs.PostDtos;
using foroLIS_backend.Infrastructure.Context;
using foroLIS_backend.Migrations;
using foroLIS_backend.Models;
using foroLIS_backend.Services;
using MercadoPago.Resource.User;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

namespace foroLIS_backend.Repository
{
    public class CommunityMessagesRepository: ICommunityMessagesRepository<CommunityMessage, CommunityMessageDto>
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CommunityMessagesRepository(ApplicationDbContext context,
            IUserService userService,
            IHttpContextAccessor httpContextAccessor) {
            _context = context;
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task Add(CommunityMessage cm) =>
            await _context.CommunityMessages.AddAsync(cm);

        public void Delete(CommunityMessage cm)
        {
            var userFields = _context.CommunityUserFields
                .Include(cuf => cuf.CommunityFieldSurvey)
                .ThenInclude(cfs => cfs.Survey)
                .Where(s => s.CommunityFieldSurvey.SurveyId == cm.Id);
            var fields = _context.CommunityFields
                .Where(cf => cf.SurveyId == cm.Id);
            var likes = _context.CommunityLikes.Where(cl => cl.CommunityMessageId == cm.Id);
            var comments = _context.CommunityMessageComments.Where(cl => cl.CommunityMessageId == cm.Id);
            
            _context.CommunityUserFields.RemoveRange(userFields);
            _context.CommunityFields.RemoveRange(fields);
            _context.CommunityLikes.RemoveRange(likes);
            _context.CommunityMessageComments.RemoveRange(comments);
            _context.CommunityMessages.Remove(cm);
            _context.SaveChanges();
        }



        public async Task<PaginatedResponse<CommunityMessageDto>> GetPaginatedAsync(Guid postId, int page = 1, int pageSize = 10)
        {
            var baseUrl = _httpContextAccessor.HttpContext?.Request.Host.ToString();
            var user = await _userService.GetCurrentUserAsync();
            var isAuthorized = await _context.Donations
             .AnyAsync(d => d.PostId == postId && (d.DonorId == user.Id || d.ReceiverId == user.Id));

            if (!isAuthorized)
            {
                return null;
            }

            var query = _context.CommunityMessages
           .Where(cm => cm.PostId == postId)
           .Include(cm => cm.Survey)
               .ThenInclude(s => s.Fields)
           .Include(cm => cm.User)
           .Include(cm => cm.Post)
               .ThenInclude(p => p.User);

            var totalItems = await query.CountAsync();
            
            var items = await query
             .OrderByDescending(cm => cm.Fecha)
             .Skip((page - 1) * pageSize)
             .Take(pageSize)
             .Select(cm => new CommunityMessageDto
             {
                 Id = cm.Id,
                 Texto = cm.Texto,
                 Fecha = cm.Fecha,
                 UserId = cm.UserId,
                 UserName = cm.User.UserName,
                 isOwner = cm.Post.User.Id == user.Id,
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
                 likes = _context.CommunityLikes.Count(cl => cl.CommunityMessageId == cm.Id),
                 isLiked = _context.CommunityLikes.Any(cl => cl.CommunityMessageId == cm.Id && cl.UserId == user.Id),
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
             })
             .ToListAsync();


            return new PaginatedResponse<CommunityMessageDto>
            {
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems,
                Items = items
                
            };
        }

        public async Task<UpdateLikeDto> LikeUnlike(Guid CommunityMessageId)
        {
            var user = await _userService.GetCurrentUserAsync(); 
            var cl = await _context.CommunityLikes.FirstOrDefaultAsync(cl => cl.CommunityMessageId == CommunityMessageId && cl.UserId== user.Id);
            if(cl == null)
            {
                var newLike = new CommunityLikes
                {
                    CommunityMessageId = CommunityMessageId,
                    UserId = user.Id
                };
                await _context.CommunityLikes.AddAsync(newLike);
                await _context.SaveChangesAsync();
                return new UpdateLikeDto
                {
                    CommunityMessageId = CommunityMessageId,
                    UserId = user.Id,
                    option = "add"
                };
            }
            _context.CommunityLikes.Remove(cl);
            await _context.SaveChangesAsync();
            return new UpdateLikeDto
            {
                CommunityMessageId = CommunityMessageId,
                UserId = user.Id,
                option = "remove"
            };

        }

        public void Update(CommunityMessage cm)
        {
            _context.CommunityMessages.Attach(cm);
            _context.CommunityMessages.Entry(cm).State = EntityState.Modified;
        }

        public async Task<CommunityMessage> GetById(Guid id)
            => await _context.CommunityMessages
                .Include(cm => cm.Post)
                    .ThenInclude(p => p.User)
                .Include(cm => cm.User)
                .FirstAsync(cm => cm.Id == id);



        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }



    }
}

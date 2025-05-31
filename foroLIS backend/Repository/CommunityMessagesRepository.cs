using foroLIS_backend.DTOs;
using foroLIS_backend.DTOs.PostDtos;
using foroLIS_backend.Infrastructure.Context;
using foroLIS_backend.Migrations;
using foroLIS_backend.Services;
using MercadoPago.Resource.User;
using Microsoft.EntityFrameworkCore;

namespace foroLIS_backend.Repository
{
    public class CommunityMessagesRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserService _userService;
        public CommunityMessagesRepository(ApplicationDbContext context, IUserService userService) {
            _context = context;
            _userService = userService;
        }

        public async Task<PaginatedResponse<CommunityMessageDto>> GetPaginatedAsync(Guid postId, int page = 1, int pageSize = 10)
        {
            var user = await _userService.GetCurrentUserAsync();
            var isAuthorized = await _context.Donations
             .AnyAsync(d => d.PostId == postId && (d.DonorId == user.Id || d.ReceiverId == user.Id));

            if (!isAuthorized)
            {
                return null;
            }

            var query = _context.CommunityMessages
                .Where(cm => cm.PostId == postId);

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
                    UserName = cm.User.UserName
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



    }
}

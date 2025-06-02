using FluentValidation;
using foroLIS_backend.DTOs.CommunityMessagesDto;
using foroLIS_backend.Infrastructure.Context;
using foroLIS_backend.Services;
using Microsoft.EntityFrameworkCore;

namespace foroLIS_backend.Validators
{
    public class GetCommunityMessageValidator:AbstractValidator<CommunityMessagePaginatedDto>
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserService _userService;
        public GetCommunityMessageValidator(ApplicationDbContext context, IUserService userService) { 
            _context = context; 
            _userService = userService;
            RuleFor(r => r.postId).MustAsync(isIntoCM).WithMessage("No estas autorizado para ver los posts en esta comunidad");
        }
        private async Task<bool> isIntoCM(Guid postId, CancellationToken cancellationToken)
        {
            var user = await _userService.GetCurrentUserAsync();
            bool isOwner = await _context.Posts.AnyAsync(p => p.User.Id == user.Id);
            bool isMember = await _context.Donations.AnyAsync(don => don.Post.Id == postId && don.DonorId == user.Id);
            Console.WriteLine(isOwner ? "es admin" : "no es admin");
            Console.WriteLine(isMember ? "es miembro" : "no es miembro");
            return isOwner || isMember;
        }
    }
}

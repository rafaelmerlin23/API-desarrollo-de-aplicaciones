using FluentValidation;
using foroLIS_backend.DTOs.CommunityMessagesDto;
using foroLIS_backend.Infrastructure.Context;
using foroLIS_backend.Services;
using Microsoft.EntityFrameworkCore;

namespace foroLIS_backend.Validators
{
    public class CommunityMessageValidator: AbstractValidator<CreateCommunityMessagesDto>
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserService _userService;

        public CommunityMessageValidator(ApplicationDbContext applicationContext, IUserService userService)
        {
            _context = applicationContext;
            _userService = userService;
            RuleFor(r => r.PostId).NotEmpty().MustAsync(PostExists).WithMessage("PostNoExists");
            RuleFor(r => r.Texto).MinimumLength(10).WithMessage("El contenido debe tener al menos 10 caracteres");
            RuleFor(r => r.PostId).MustAsync(isIntoCM).WithMessage("No estas autorizado para crear posts en esta comunidad");

        }
        // que el usuario sea miembro de la comunidad (que haya donado al menos una vez) o sea el creador del post
        private async Task<bool> isIntoCM(Guid postId, CancellationToken cancellationToken)
        {
            var user = await _userService.GetCurrentUserAsync();
            bool isOwner = await _context.Posts.AnyAsync(p => p.User.Id == user.Id);
            bool isMember = await _context.Donations.AnyAsync(don => don.Post.Id == postId && don.DonorId == user.Id);
            Console.WriteLine(isOwner? "es admin":"no es admin");
            Console.WriteLine(isMember ? "es miembro" : "no es miembro");
            return isOwner || isMember;
        }
        // que exista el post
        private async Task<bool> PostExists(Guid postId, CancellationToken cancellationToken)
        {
            return await _context.Posts.AnyAsync(p=> p.Id == postId);
        }
    }
}

using FluentValidation;
using foroLIS_backend.DTOs.CommunityMessagesDto;
using foroLIS_backend.Infrastructure.Context;
using foroLIS_backend.Models;
using foroLIS_backend.Services;
using Microsoft.EntityFrameworkCore;

namespace foroLIS_backend.Validators
{
    public class UpdateCommunityMessageValidator:AbstractValidator<UpdateCommunityMessageDto>
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserService _userService;
        public UpdateCommunityMessageValidator(ApplicationDbContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
            RuleFor(r => r.Id).MustAsync(isOwner).WithMessage("No estas autorizado para editar este post en esta comunidad");
            RuleFor(r =>r.Texto).MinimumLength(10).WithMessage("El contenido debe tener al menos 10 caracteres");
        }

        private async Task<bool> isOwner(Guid id, CancellationToken cancellationToken)
        {
            var user = await _userService.GetCurrentUserAsync();

            CommunityMessage cm = await _context.CommunityMessages.Include(cm => cm.Post)
                 .ThenInclude(p => p.User)
                .Include(cm => cm.User)
                .FirstAsync(cm => cm.Id == id);
            if (cm == null)
            {
                return false;
            }
            return cm.User.Id == user.Id;
        }
    }
}

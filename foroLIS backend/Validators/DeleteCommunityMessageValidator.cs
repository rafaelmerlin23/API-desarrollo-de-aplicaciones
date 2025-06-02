using FluentValidation;
using foroLIS_backend.DTOs.CommunityMessagesDto;
using foroLIS_backend.Infrastructure.Context;
using foroLIS_backend.Models;
using foroLIS_backend.Services;
using Microsoft.EntityFrameworkCore;

namespace foroLIS_backend.Validators
{
    public class DeleteCommunityMessageValidator:AbstractValidator<DeleteCommunityMessageDto>
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserService _userService;

        public DeleteCommunityMessageValidator(
            ApplicationDbContext context
            , IUserService userService
            ) { 
            _context = context;
            _userService = userService;

            RuleFor(r => r.Id).MustAsync(isOwnerOrCreator).WithMessage("no puedes eliminar este mensaje no tienes permisos");
        }

        private async Task<bool> isOwnerOrCreator(Guid id, CancellationToken cancellationToken)
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
            bool isOwner  = cm.User.Id == user.Id;
            bool isCreator = cm.Post.User.Id == user.Id ;
            return isOwner || isCreator;
        }
    }
}

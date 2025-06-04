using FluentValidation;
using foroLIS_backend.DTOs.FileDto;
using foroLIS_backend.Infrastructure.Context;
using foroLIS_backend.Services;
using Microsoft.EntityFrameworkCore;

namespace foroLIS_backend.Validators
{
    public class FIleCommunityMessageValidator: AbstractValidator<AddCommunityMessageFileDto>
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserService _userService;
        public FIleCommunityMessageValidator(ApplicationDbContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
            RuleFor(fp => fp.CommunityMessageId).MustAsync(isIntoCM).WithMessage("No estás autorizado para agregar imagenes en este mensaje  de la comunidad");
            RuleFor(fp => fp.MediaFileId).NotNull().WithMessage("debes de ingresar el MediaFieldId").MustAsync(existsFile).WithMessage("No existe ese archivo");
            RuleFor(fp => fp.CommunityMessageId).NotNull().WithMessage("debes de ingresar el CommunityMessageId").MustAsync(existsCommunityMessage).WithMessage("No existe el CommunityMessage");

        }

        private async Task<bool> existsFile(Guid fileId, CancellationToken cancellationToken)
        {
            var file = _context.MediaFiles.FirstOrDefaultAsync(f => f.Id == fileId);
            return file != null ? true : false;
        }

        private async Task<bool> existsCommunityMessage(Guid CommunityMessageId, CancellationToken cancellationToken)
        {
            var file = _context.CommunityMessages.FirstOrDefaultAsync(f => f.Id == CommunityMessageId);
            return file != null ? true : false;
        }

        private async Task<bool> isIntoCM(Guid CommunityMessageId, CancellationToken cancellationToken)
        {

            var user = await _userService.GetCurrentUserAsync();
            
            var cm = await _context.CommunityMessages
                .Include(cm=> cm.Post)
                .FirstOrDefaultAsync(cm => cm.Id == CommunityMessageId);
            if (cm == null) {
                return false;
            }
            bool isOwner = await _context.Posts.AnyAsync(p => p.User.Id == user.Id && p.Id == cm.PostId);
            bool isMember = await _context.Donations.AnyAsync(don => don.Post.Id == cm.PostId && don.DonorId == user.Id);
            Console.WriteLine(isOwner ? "es admin" : "no es admin");
            Console.WriteLine(isMember ? "es miembro" : "no es miembro");
            return isOwner || isMember;
        }
    }

}

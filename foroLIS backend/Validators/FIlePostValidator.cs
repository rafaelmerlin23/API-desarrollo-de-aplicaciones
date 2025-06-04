using FluentValidation;
using foroLIS_backend.DTOs.FileDto;
using foroLIS_backend.Infrastructure.Context;
using foroLIS_backend.Services;
using Microsoft.EntityFrameworkCore;

namespace foroLIS_backend.Validators
{
    public class FIlePostValidator:AbstractValidator<AddPostFileDto>
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserService _userService;
        public FIlePostValidator(ApplicationDbContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
            RuleFor(fp => fp.PostId).MustAsync(isIntoCM).WithMessage("No estás autorizado para agregar imagenes en este mensaje  de la comunidad");
            RuleFor(fp => fp.PostId).NotEmpty().WithMessage("Tienes que agregar un postId").MustAsync(existsPost).WithMessage("No existe este posts");
            RuleFor(fp => fp.FileId).NotEmpty().WithMessage("Tienes que agregar un fileId").MustAsync(existsFile).WithMessage("El archivo no existe");
        }

        private async Task<bool> existsFile(Guid fileId, CancellationToken cancellationToken)
        {
            var file = _context.MediaFiles.FirstOrDefaultAsync(f => f.Id == fileId);
            return file != null ? true : false;
        }
        private async Task<bool> existsPost(Guid postId , CancellationToken cancellationToken)
        {
            var post = await _context.Posts.FirstOrDefaultAsync(p =>  p.Id == postId);
            return post != null ? true : false;
        }
        private async Task<bool> isIntoCM(Guid postId, CancellationToken cancellationToken)
        {
            var user = await _userService.GetCurrentUserAsync();
            bool isOwner = await _context.Posts.AnyAsync(p => p.User.Id == user.Id && p.Id == postId);
            bool isMember = await _context.Donations.AnyAsync(don => don.Post.Id == postId && don.DonorId == user.Id);
            Console.WriteLine(isOwner ? "es admin" : "no es admin");
            Console.WriteLine(isMember ? "es miembro" : "no es miembro");
            return isOwner || isMember;
        }
    }
}

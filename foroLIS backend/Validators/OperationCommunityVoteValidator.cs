using FluentValidation;
using foroLIS_backend.DTOs.CommunitySurveyDtos;
using foroLIS_backend.Infrastructure.Context;
using foroLIS_backend.Repository;
using foroLIS_backend.Services;
using Microsoft.EntityFrameworkCore;

namespace foroLIS_backend.Validators
{
    public class OperationCommunityVoteValidator : AbstractValidator<OperationCommunityVoteDto>
    {
        private readonly ICommunitySurveyRepository _surveyRepository;
        private readonly IUserService _userService;
        private readonly ApplicationDbContext _context;

        public OperationCommunityVoteValidator(
            ICommunitySurveyRepository surveyRepository,
            IUserService userService,
            ApplicationDbContext context)
        {
            _surveyRepository = surveyRepository;
            _userService = userService;
            _context = context;

            // Validación base
            RuleFor(sv => sv.FieldId).NotEmpty().WithMessage("El campo FieldId es obligatorio.");
            RuleFor(sv => sv.Operation)
                .NotEmpty().WithMessage("La operación es obligatoria.")
                .Must(op => op == "add" || op == "remove")
                .WithMessage("Operación inválida. Debe ser 'add' o 'remove'.");

            // Reglas condicionales según la operación
            RuleFor(sv => sv).MustAsync(IsUserAllowed).WithMessage("No tienes permiso para interactuar en esta encuesta.");
            RuleFor(sv => sv).MustAsync(NotExpired).WithMessage("Esta encuesta está cerrada.");

            When(sv => sv.Operation == "add", () =>
            {
                RuleFor(sv => sv).MustAsync(IsValidVote)
                    .WithMessage("No puedes seleccionar más de un campo en esta encuesta.");
            });

            When(sv => sv.Operation == "remove", () =>
            {
                RuleFor(sv => sv).MustAsync(UserHasVoted)
                    .WithMessage("No has votado este campo.");
            });
        }

        private async Task<bool> NotExpired(OperationCommunityVoteDto request, CancellationToken cancellationToken)
        {
            var field = await _context.CommunityFields
                .Include(cf => cf.Survey)
                .FirstOrDefaultAsync(cf => cf.Id == request.FieldId, cancellationToken);

            return field != null && !_surveyRepository.IsSurveyExpired(field.Survey);
        }

        private async Task<bool> IsValidVote(OperationCommunityVoteDto request, CancellationToken cancellationToken)
        {
            var field = await _context.CommunityFields
                .Include(cf => cf.Survey)
                .FirstOrDefaultAsync(cf => cf.Id == request.FieldId, cancellationToken);

            if (field == null) return false;

            var user = await _userService.GetCurrentUserAsync();
            int numberOfVotes = await _context.CommunityUserFields.CountAsync(cuf => cuf.UserId == user.Id );
            Console.WriteLine(numberOfVotes);
            return field.Survey.AllowMoreOneAnswer == true || numberOfVotes < 1;
        }

        private async Task<bool> UserHasVoted(OperationCommunityVoteDto request, CancellationToken cancellationToken)
        {
            var user = await _userService.GetCurrentUserAsync();
            return await _context.CommunityUserFields
                .AnyAsync(v => v.CommunityFieldId == request.FieldId && v.UserId == user.Id, cancellationToken);
        }

        private async Task<bool> IsUserAllowed(OperationCommunityVoteDto request, CancellationToken cancellationToken)
        {
            var field = await _context.CommunityFields
                .Include(cf => cf.Survey)
                    .ThenInclude(s => s.CommunityMessage)
                        .ThenInclude(cm => cm.Post)
                .FirstOrDefaultAsync(f => f.Id == request.FieldId, cancellationToken);

            if (field == null) return false;

            var user = await _userService.GetCurrentUserAsync();
            var postId = field.Survey.CommunityMessage.Post.Id;

            bool isOwner = await _context.Posts.AnyAsync(p => p.Id == postId && p.User.Id == user.Id, cancellationToken);
            bool isMember = await _context.Donations.AnyAsync(d => d.Post.Id == postId && d.DonorId == user.Id, cancellationToken);

            return isOwner || isMember;
        }
    }
}

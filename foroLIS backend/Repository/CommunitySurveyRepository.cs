using Azure.Core;
using foroLIS_backend.DTOs.CommunitySurveyDtos;
using foroLIS_backend.Infrastructure.Context;
using foroLIS_backend.Migrations;
using foroLIS_backend.Models;
using foroLIS_backend.Services;
using Microsoft.EntityFrameworkCore;

namespace foroLIS_backend.Repository
{
    public class CommunitySurveyRepository: ICommunitySurveyRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserService _userService;
        public CommunitySurveyRepository(
            ApplicationDbContext context,
            IUserService userService
            ) {
            _context = context;
            _userService = userService;
        }

        public async Task<CommunitySurveyDto?> CreateSurvey(CreateCommunitySurvey request)
        {
            if (request.Fields.Count < 2)
            {
                return null;
            }

            var endDate = request.EndDate ?? DateTime.UtcNow.AddDays(7);

            var newCommunitySurvey = new CommunitySurvey
            {
                Id = request.CommunityMessageId,
                Title = request.Title,
                AllowMoreOneAnswer = request.AllowMoreOneAnswer,
                EndDate = endDate
            };

            var fields = request.Fields.Select(f => new CommunityFieldSurvey
            {
                Title = f.Title,
                SurveyId = request.CommunityMessageId 
            }).ToList();

            await _context.CommunitySurveys.AddAsync(newCommunitySurvey);
            await _context.CommunityFields.AddRangeAsync(fields);
            await _context.SaveChangesAsync();

            var fieldsDto = fields.Select(f => new CommunityFieldDto
            {
                Title = f.Title,
                Id = f.Id,
                isvote = false,
                SurveyId = f.SurveyId,
                votes = 0,
            }).ToList();

            return new CommunitySurveyDto
            {
                CommunityMessageId = request.CommunityMessageId,
                AllowMoreOneAnswer = request.AllowMoreOneAnswer,
                Title = request.Title,
                Fields = fieldsDto,
                EndDate = endDate
            };
        }

        public async Task<VoteDto> AddVote(OperationCommunityVoteDto request)
        {
            var user = await _userService.GetCurrentUserAsync();

            var vote = new CommunityFieldsUser
            {
                CommunityFieldId = request.FieldId,
                UserId = user.Id
            };

            _context.CommunityUserFields.Add(vote);
            await _context.SaveChangesAsync();

            return new VoteDto
            {
                Action = "Add",
                FieldId = request.FieldId,
                userName = user.FullName
            };
        }

        public async Task<VoteDto> RemoveVote(OperationCommunityVoteDto request)
        {
            var user = await _userService.GetCurrentUserAsync();

            var vote = await _context.CommunityUserFields
                .FirstOrDefaultAsync(v => v.CommunityFieldId == request.FieldId && v.UserId == user.Id);

            if (vote != null)
            {
                _context.CommunityUserFields.Remove(vote);
                await _context.SaveChangesAsync();
            }

            return new VoteDto
            {
                Action = "Remove",
                FieldId = request.FieldId,
                userName = user.FullName
            };
        }


        public bool IsSurveyExpired(CommunitySurvey survey)
        {
            return survey.EndDate <= DateTime.UtcNow;
        }


        public async Task<CommunityFieldsUser?> IsVoteExists(Guid fieldId, string userId)
        {
            return await _context.CommunityUserFields
                .FirstOrDefaultAsync(cuf => cuf.UserId == userId && cuf.CommunityFieldId == fieldId);
        }

        public async Task<int> GetNumberOfVotesCurrentUser(Guid fieldId, string userId)
        {
            return await _context.CommunityUserFields
                .CountAsync(cm => cm.UserId == userId && cm.CommunityFieldSurvey.Id == fieldId);
        }

        public async Task Save() =>  await _context.SaveChangesAsync();
        
    }
}

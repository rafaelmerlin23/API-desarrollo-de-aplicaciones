using foroLIS_backend.DTOs.CommunitySurveyDtos;
using foroLIS_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace foroLIS_backend.Repository
{
    public interface ICommunitySurveyRepository
    {
        public Task<CommunitySurveyDto?> CreateSurvey(CreateCommunitySurvey request);

        public Task<VoteDto> AddVote(OperationCommunityVoteDto request);
        public Task<VoteDto> RemoveVote(OperationCommunityVoteDto request);

        public bool IsSurveyExpired(CommunitySurvey survey);


        public Task<CommunityFieldsUser?> IsVoteExists(Guid fieldId, string userId);

        public  Task<int> GetNumberOfVotesCurrentUser(Guid fieldId, string userId);

        public Task Save();
    }
}

using foroLIS_backend.DTOs.CommunitySurveyDtos;
using foroLIS_backend.Repository;

namespace foroLIS_backend.Services
{
    public class CommunitySurveyService
    {
        private readonly ICommunitySurveyRepository _surveyRepository;
        public CommunitySurveyService(ICommunitySurveyRepository surveyRepository)
        {
            _surveyRepository = surveyRepository;
        }

        
        public async Task<CommunitySurveyDto?> CreateSurvey(CreateCommunitySurvey request)
        {
            var response = await _surveyRepository.CreateSurvey(request);
            return response;
        }

        public async Task<VoteDto> AddVote(OperationCommunityVoteDto request)
        {
            var response = await _surveyRepository.AddVote(request);
            return response;
        }

        public async Task<VoteDto> RemoveVote(OperationCommunityVoteDto request)
        {
            var response = await _surveyRepository.RemoveVote(request);
            return response;
        }
    }
}

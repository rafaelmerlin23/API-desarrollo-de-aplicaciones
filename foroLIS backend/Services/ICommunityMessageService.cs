using foroLIS_backend.DTOs;
using foroLIS_backend.DTOs.CommunityMessagesDto;
using foroLIS_backend.Models;
using foroLIS_backend.Repository;

namespace foroLIS_backend.Services
{
    public interface ICommunityMessageService<TDTO,TI,TU,TD>
    {
        public Task<PaginatedResponse<TDTO>> GetPaginatedAsync(Guid postId, int page = 1, int pageSize = 10);


        public Task<TDTO> Delete(TD request);

        public Task<TDTO> GetById(Guid id);

        public Task<TDTO> Update(TU request);

        public Task<CommunityMessageDto> Add(TI request);
        
    }
}

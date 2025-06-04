using foroLIS_backend.DTOs;
using foroLIS_backend.DTOs.CommunityMessagesDto;
using foroLIS_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace foroLIS_backend.Repository
{
    public interface ICommunityMessagesRepository<T,TDTO>
    {
        public Task Add(T cm);

        public void Delete(T cm);

        
        public Task<PaginatedResponse<TDTO>> 
            GetPaginatedAsync(Guid postId, int page = 1, int pageSize = 10);


        public void Update(T cm);


        public Task Save();

        public  Task<T> GetById(Guid id);

        public  Task<UpdateLikeDto> LikeUnlike(Guid CommunityMessageId);

    }
}

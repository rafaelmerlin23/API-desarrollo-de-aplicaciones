using Azure.Core;
using foroLIS_backend.DTOs;
using foroLIS_backend.DTOs.CommunityMessagesDto;
using foroLIS_backend.DTOs.PostDtos;
using foroLIS_backend.Models;
using foroLIS_backend.Repository;
using MercadoPago.Resource.User;
using System.Threading.Tasks;

namespace foroLIS_backend.Services
{
    public class CommunityMessageService: ICommunityMessageService<
        CommunityMessageDto,
        CreateCommunityMessagesDto,
        UpdateCommunityMessageDto,
        DeleteCommunityMessageDto>
    {
        private readonly ICommunityMessagesRepository<CommunityMessage,CommunityMessageDto> _repository;
        private readonly IUserService _userService;
        IRepository<Post, PostDto> _postRepository;
        public CommunityMessageService(
            ICommunityMessagesRepository<CommunityMessage, CommunityMessageDto> repository,
        IUserService userService,
        IRepository<Post, PostDto> postRepository) { 
            _repository = repository;
            _userService = userService;   
            _postRepository = postRepository;
        }

        public async Task<PaginatedResponse<CommunityMessageDto>> GetPaginatedAsync(Guid postId, int page = 1, int pageSize = 10)
        {
            var Cms = await _repository.GetPaginatedAsync(postId, page, pageSize);
            return Cms;
        }

        public async Task<CommunityMessageDto> Delete(DeleteCommunityMessageDto request)
        {
            var user = await _userService.GetCurrentUserAsync();
            var cm = await _repository.GetById(request.Id);
            var dto = cm.ToDto();
            var userName = cm.User.UserName;
            var isOwner = cm.Post.User.Id == user.Id;
            dto.UserName = userName ?? "NA";
            dto.isOwner = isOwner;
            
            _repository.Delete(cm);
            await _repository.Save();
            
            return dto;
        }

        public async Task<CommunityMessageDto> GetById(Guid id)
        {
            var user = await _userService.GetCurrentUserAsync();
            var cm = await _repository.GetById(id);
            var dto = cm.ToDto();
            var userName = cm.User.UserName;
            var isOwner = cm.Post.User.Id == user.Id;
            dto.UserName = userName ?? "NA";
            dto.isOwner = isOwner;
            return dto;
        }

        public async Task<CommunityMessageDto> Update(UpdateCommunityMessageDto request)
        {
            var user = await _userService.GetCurrentUserAsync();
            var cm = await _repository.GetById(request.Id);
            cm.Texto = request.Texto;
            _repository.Update(cm);
            await _repository.Save();
            var dto = cm.ToDto();
            var userName = cm.User.UserName;
            var isOwner = cm.Post.User.Id == user.Id;
            dto.UserName = userName ?? "NA";
            dto.isOwner = isOwner;
            return dto;
        }

        public async Task<CommunityMessageDto> Add(CreateCommunityMessagesDto request)
        {
            var user = await _userService.GetCurrentUserAsync();
            var post = await _postRepository.GetById(request.PostId);
            var cm = new CommunityMessage
            {
                Fecha = DateTime.UtcNow,
                PostId = request.PostId,
                UserId = user.Id,
                Texto = request.Texto,
            };

            await _repository.Add(cm);
            await _repository.Save();

            var dto = cm.ToDto();
            dto.UserName = user.FullName ?? "NA";
            dto.isOwner = post.User.Id == user.Id;
            return dto;


        }
    }
}

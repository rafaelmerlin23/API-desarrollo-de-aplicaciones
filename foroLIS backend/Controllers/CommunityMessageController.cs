using FluentValidation;
using foroLIS_backend.DTOs;
using foroLIS_backend.DTOs.CommunityMessagesDto;
using foroLIS_backend.DTOs.PostDtos;
using foroLIS_backend.Services;
using foroLIS_backend.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace foroLIS_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommunityMessageController : ControllerBase
    {
        private readonly ICommunityMessageService<CommunityMessageDto, 
            CreateCommunityMessagesDto, UpdateCommunityMessageDto, 
            DeleteCommunityMessageDto> _cmService;

        private readonly IValidator<CreateCommunityMessagesDto> _ccmValidator;
        private readonly IValidator<CommunityMessagePaginatedDto> _getCcmValidator;
        private readonly IValidator<UpdateCommunityMessageDto> _updateCommunityMessageValidator;
        private readonly IValidator<DeleteCommunityMessageDto> _deleteCommunityMessageValidator;    

        public CommunityMessageController(ICommunityMessageService<CommunityMessageDto,
            CreateCommunityMessagesDto, UpdateCommunityMessageDto,
            DeleteCommunityMessageDto> cmService,
            IValidator<CreateCommunityMessagesDto> ccmValidator,
            IValidator<CommunityMessagePaginatedDto> getCcmValidator,
            IValidator<UpdateCommunityMessageDto> updateCommunityMessageValidator,
            IValidator<DeleteCommunityMessageDto> deleteCommunityMessageValidator) { 
            _cmService = cmService;
            _ccmValidator = ccmValidator;
            _getCcmValidator = getCcmValidator;
            _updateCommunityMessageValidator = updateCommunityMessageValidator;
            _deleteCommunityMessageValidator = deleteCommunityMessageValidator;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<PaginatedResponse<CommunityMessageDto>>> Get([FromQuery] CommunityMessagePaginatedDto request)
        {

            try
            {
                var validationResult = await _getCcmValidator.ValidateAsync(request);
                if(!validationResult.IsValid)
                {
                    return BadRequest(validationResult.Errors);
                }
                var result = await _cmService.GetPaginatedAsync(request.postId,request.page,request.pageSize);
                return result;

            }catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize]
        [HttpPut]
        public async Task<ActionResult<CommunityMessageDto>> Update(UpdateCommunityMessageDto request)
        {
            try
            {
                var validationResult = await _updateCommunityMessageValidator.ValidateAsync(request);
                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult.Errors);
                }
                var result = await _cmService.Update(request);

                return result;

            }
            catch (Exception ex)
            {
                    return StatusCode(500, ex.Message);
            } 
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<CommunityMessageDto>> Add(CreateCommunityMessagesDto request)
        {
            try
            {
                var validationResult = await _ccmValidator.ValidateAsync(request);

                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult.Errors);
                }
                var cm = await _cmService.Add(request);
                return cm;
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [Authorize]
        [HttpDelete]
        public async Task<ActionResult<Message<CommunityMessageDto>>> Delete(DeleteCommunityMessageDto request)
        {
            try
            {
                var validationResult = await _deleteCommunityMessageValidator.ValidateAsync(request);
                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult.Errors);
                }
                var result = await _cmService.Delete(request);
                return new Message<CommunityMessageDto>
                {
                    data= result,
                    message = "Mensaje correctamente eliminado"
                };
            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);

            }
        }
    }

}

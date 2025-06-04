using FluentValidation;
using foroLIS_backend.DTOs;
using foroLIS_backend.DTOs.CommunityMessagesDto;
using foroLIS_backend.DTOs.CommunitySurveyDtos;
using foroLIS_backend.DTOs.FileDto;
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
        private readonly IValidator<OperationCommunityVoteDto> _operationCommunityVoteValidator;
        private readonly CommunitySurveyService _communitySurveyService;
        private readonly FileService _fileService;

        public CommunityMessageController(ICommunityMessageService<CommunityMessageDto,
            CreateCommunityMessagesDto, UpdateCommunityMessageDto,
            DeleteCommunityMessageDto> cmService,
            IValidator<CreateCommunityMessagesDto> ccmValidator,
            IValidator<CommunityMessagePaginatedDto> getCcmValidator,
            IValidator<UpdateCommunityMessageDto> updateCommunityMessageValidator,
            IValidator<DeleteCommunityMessageDto> deleteCommunityMessageValidator,
            CommunitySurveyService communitySurveyService,
            IValidator<OperationCommunityVoteDto> operationCommunityVoteValidator,
            FileService fileService) { 
            _cmService = cmService;
            _ccmValidator = ccmValidator;
            _getCcmValidator = getCcmValidator;
            _updateCommunityMessageValidator = updateCommunityMessageValidator;
            _deleteCommunityMessageValidator = deleteCommunityMessageValidator;
            _communitySurveyService = communitySurveyService;
            _operationCommunityVoteValidator = operationCommunityVoteValidator;
            _fileService = fileService;
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
        [HttpPut("Like")]
        public async Task<ActionResult<Message<UpdateLikeDto>>> LikeUnlike(LikeUnlikeDto request)
        {
            try
            {
                var response = await _cmService.LikeUnlike(request.CommunityMessageId);
                return new Message<UpdateLikeDto>
                {
                    data = response,
                    message = response.option == "add" ? "like añadido con exito" : "like eliminado con exito"
                };
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize]
        [HttpPost("add-file")]
        public async Task<ActionResult<AddCommunityMessageFileDto>> AddFileToCommunityMessage(AddCommunityMessageFileDto request)
        {
            try
            {
                var response = await _fileService.AddFileToCommunityMessage(request);
                return response;
            }catch(Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
        }

        [Authorize]
        [HttpPut("vote")]
        public async Task<ActionResult<Message<VoteDto>>> Vote(OperationCommunityVoteDto request)
        {
            var validationResult = await _operationCommunityVoteValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            VoteDto response = request.Operation == "remove"
                ? await _communitySurveyService.RemoveVote(request)
                : await _communitySurveyService.AddVote(request);

            return new Message<VoteDto>
            {
                data = response,
                message = response.Action == "Remove" ? "Voto removido" : "Voto añadido"
            };
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

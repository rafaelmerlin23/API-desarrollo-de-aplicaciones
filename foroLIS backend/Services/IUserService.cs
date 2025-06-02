using foroLIS_backend.DTOs;

namespace foroLIS_backend.Services
{
    public interface IUserService
    {
        Task<UserResponseDto> RegisterAsync(UserRegisterRequestDto request, GoogleUserDto googleUser);
        Task<UserResponseDto> RegisterAsync(UserRegisterRequestDto2 request);
        Task<CurrentUserResponseDto> GetCurrentUserAsync();
        Task<UserResponseDto> GoogleLoginAsync(UserRegisterRequestDto request);
        Task<UserResponseDto> GetByIdAsync(string id);
        Task<UserResponseDto> UpdateAsync(UpdateUserDto request);
        Task DeleteAsync(string id);
        Task<RevokeRefreshTokenResponseDto> RevokeRefreshToken(RefreshTokenRequestDto refreshTokenRemoveRequest);
        Task<CurrentUserResponseDto> RefreshTokenAsync(RefreshTokenRequestDto request);

        Task<UserResponseDto> LoginAsync(UserLoginRequestDto request);
        Task<UserResponseDto> LoginAsync(UserLoginRequestDto2 request);
    }
}

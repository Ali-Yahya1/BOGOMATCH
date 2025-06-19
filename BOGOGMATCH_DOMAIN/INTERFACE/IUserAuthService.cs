using AngularAuthAPI.Models.Dto;
using BOGOGMATCH_DOMAIN.MODELS.UserManagement;

namespace BOGOMATCH_DOMAIN.INTERFACE
{
    public interface IUserAuthService
    {
        Task<TokenApiDTO> AuthenticateAsync(User userObj);
        Task<string> RegisterUserAsync(User userObj);
        Task<List<User>> GetAllUsersAsync();
        Task<TokenApiDTO> RefreshTokenAsync(TokenApiDTO TokenApiDTO);
        Task<string> SendResetEmailAsync(string email);
        Task<string> ResetPasswordAsync(ResetPasswordDTO resetPasswordDto);
    }
}

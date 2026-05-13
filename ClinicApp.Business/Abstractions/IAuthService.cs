using ClinicApp.Business.Dtos.Authentication;

namespace ClinicApp.Business.Abstractions
{
    public interface IAuthService
    {
        Task<bool> RegisterAsync(RegisterDto dto);

        Task<bool> LoginAsync(LoginDto dto);

        Task LogoutAsync();
    }
}
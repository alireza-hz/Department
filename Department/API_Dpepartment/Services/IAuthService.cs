using API_Dpepartment.Models.Auth;

namespace API_Dpepartment.Services
{
    public interface IAuthService
    {
        AuthResponseDto Register(RegisterDto dto);
        AuthResponseDto? Login(LoginDto dto);

    }
}
